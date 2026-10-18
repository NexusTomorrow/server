using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Ops;

namespace NexusTomorrow.Commands;

/// <summary>
/// Runs command lines: splits the line, finds the command, checks its arguments, permission and console-only rule, runs it, and
/// queues one audit row for every command that resolved, denials included. A secret parameter's value, and any token that no
/// parameter names, reaches neither the audit, a log nor a reply.
/// </summary>
public sealed class CommandDispatcher
{
    /// <summary>
    /// The built-in command that lists the commands an actor may run, with their usage. It needs no permission and works in game too;
    /// a registered command with this name takes its place.
    /// </summary>
    public const string HelpName = "help";

    /// <summary>
    /// What <see cref="HelpName"/> does.
    /// </summary>
    public const string HelpDescription = "List every command you may run, with its usage.";

    // The ops.audit column widths (OpsContext).
    private const int ActorWidth = 100;
    private const int FrontEndWidth = 20;
    private const int ActionWidth = 100;
    private const int TargetWidth = 200;
    private const int DetailsWidth = 2000;
    private const int RemoteAddressWidth = 64;

    private readonly Dictionary<string, CommandDefinition> _byName = new(StringComparer.OrdinalIgnoreCase);
    private readonly IWriteBehind _persistence;
    private readonly TimeProvider _time;
    private readonly ILogger<CommandDispatcher> _logger;

    /// <summary>
    /// Create a dispatcher over <paramref name="commands"/>, queueing audit rows on <paramref name="persistence"/>.
    /// </summary>
    public CommandDispatcher(IEnumerable<CommandDefinition> commands, IWriteBehind persistence, TimeProvider time, ILogger<CommandDispatcher> logger)
    {
        Commands = [.. commands];
        foreach (CommandDefinition command in Commands)
            _byName.Add(command.Name, command);

        _persistence = persistence;
        _time = time;
        _logger = logger;
    }

    /// <summary>
    /// The commands, in registration order.
    /// </summary>
    public IReadOnlyList<CommandDefinition> Commands { get; }

    /// <summary>
    /// Run <paramref name="line"/> as <paramref name="actor"/>. A line that does not parse, <c>help</c> and an unknown command are
    /// answered without an audit row; every other line writes exactly one.
    /// </summary>
    public async Task<CommandReply> DispatchAsync(CommandActor actor, string line, CancellationToken cancellationToken)
    {
        actor = Scrubbed(actor);
        if (!CommandLine.TrySplit(line, out List<string> tokens))
            return new CommandReply(CommandOutcome.Refused, "Unbalanced quotes.");
        if (tokens.Count == 0)
            return new CommandReply(CommandOutcome.Refused, "Nothing to run; try help.");
        if (string.Equals(tokens[0], HelpName, StringComparison.OrdinalIgnoreCase) && !_byName.ContainsKey(HelpName))
            return Help(actor);

        CommandDefinition? command = Resolve(tokens, out int nameTokens);
        if (command is null)
            return new CommandReply(CommandOutcome.Refused, "Unknown command; try help.");

        // Permission and the console rule come before the arguments, so an actor who may not run the command learns nothing of its shape.
        bool fits = TryBind(command, tokens.GetRange(nameTokens, tokens.Count - nameTokens), out List<string> arguments);
        if (command.Permission is { } permission && !actor.Holds(permission))
            return Denied(actor, command, arguments, $"You do not hold {permission}.");
        if (command.ConsoleOnly && actor.FrontEnd == FrontEnd.InGame)
            return Denied(actor, command, arguments, "That command is console only.");
        if (StorableText.IsUnstorable(line))
            return Audited(actor, command, arguments, new CommandReply(CommandOutcome.Refused, "The line holds a control character or a broken character pair."));
        if (!fits)
            return Audited(actor, command, arguments, new CommandReply(CommandOutcome.Refused, $"Usage: {command.Usage()}"));
        if (FirstInvalid(command, arguments) is { } invalid)
            return Audited(actor, command, arguments, new CommandReply(CommandOutcome.Refused, invalid.Invalid ?? $"The {invalid.Name} is not valid."));

        CommandReply reply;
        try
        {
            reply = await command.ExecuteAsync(new CommandInvocation(actor, arguments), cancellationToken);
        }
        catch (Exception exception)
        {
            CommandsLog.Failed(_logger, command.Name, exception);
            reply = new CommandReply(CommandOutcome.Failed, "The command failed; see the server log.");
        }

        return Audited(actor, command, arguments, reply);
    }

    // An actor's name and address go into the audit row and, for a suspension, into account.suspension.created_by.
    private static CommandActor Scrubbed(CommandActor actor)
    {
        string name = Clip(StorableText.Scrub(actor.Name), ActorWidth);
        string? address = actor.RemoteAddress is null ? null : Clip(StorableText.Scrub(actor.RemoteAddress), RemoteAddressWidth);
        return name == actor.Name && address == actor.RemoteAddress ? actor : actor with { Name = name, RemoteAddress = address };
    }

    private static CommandParameter? FirstInvalid(CommandDefinition command, List<string> arguments)
    {
        for (int i = 0; i < arguments.Count && i < command.Parameters.Length; i++)
        {
            if (command.Parameters[i].IsValid is { } isValid && !isValid(arguments[i]))
                return command.Parameters[i];
        }

        return null;
    }

    // The audit records a value only when it is not secret, can be stored, and passes its parameter's validator.
    private static bool Recordable(CommandParameter parameter, string value) =>
        !parameter.Secret && !StorableText.IsUnstorable(value) && (parameter.IsValid?.Invoke(value) ?? true);

    // Binds the tokens after the name to the parameters. Whether or not they fit, arguments holds only the tokens a parameter names:
    // an extra token may be the tail of an unquoted password, so it is dropped before anything can record or echo it.
    private static bool TryBind(CommandDefinition command, List<string> tokens, out List<string> arguments)
    {
        ImmutableArray<CommandParameter> parameters = command.Parameters;
        int required = parameters.Count(p => !p.Optional);
        bool rest = parameters.Length > 0 && parameters[^1].Rest;
        bool fits = tokens.Count >= required && (rest || tokens.Count <= parameters.Length);
        int named = Math.Min(tokens.Count, parameters.Length);
        arguments = tokens.GetRange(0, named);
        if (!fits)
            return false;

        if (rest && tokens.Count > parameters.Length)
        {
            int last = parameters.Length - 1;
            arguments = [.. tokens.Take(last), string.Join(' ', tokens.Skip(last))];
        }

        return true;
    }

    private CommandDefinition? Resolve(List<string> tokens, out int nameTokens)
    {
        if (tokens.Count >= 2 && _byName.TryGetValue($"{tokens[0]} {tokens[1]}", out CommandDefinition? two))
        {
            nameTokens = 2;
            return two;
        }

        nameTokens = 1;
        return _byName.GetValueOrDefault(tokens[0]);
    }

    private CommandReply Help(CommandActor actor)
    {
        var text = new StringBuilder();
        foreach (CommandDefinition command in Commands)
        {
            if (command.Permission is null || actor.Holds(command.Permission))
                text.Append(command.Usage()).Append(": ").Append(command.Description).Append('\n');
        }

        return new CommandReply(CommandOutcome.Done, text.ToString());
    }

    private CommandReply Denied(CommandActor actor, CommandDefinition command, List<string> arguments, string text)
    {
        CommandsLog.Denied(_logger, command.Name, actor.Name, actor.FrontEnd);
        return Audited(actor, command, arguments, new CommandReply(CommandOutcome.Denied, text));
    }

    private CommandReply Audited(CommandActor actor, CommandDefinition command, List<string> arguments, CommandReply reply)
    {
        // Only the arguments a parameter names, by position, that Recordable accepts.
        var details = new List<string>();
        string? target = null;
        for (int i = 0; i < arguments.Count && i < command.Parameters.Length; i++)
        {
            CommandParameter parameter = command.Parameters[i];
            if (!Recordable(parameter, arguments[i]))
                continue;
            if (i == 0)
                target = arguments[i];
            details.Add($"{parameter.Name}={arguments[i]}");
        }

        _persistence.Enqueue(new AuditWork(new AuditRow
        {
            At = _time.GetUtcNow(),
            Actor = Clip(actor.Name, ActorWidth),
            FrontEnd = Clip(actor.FrontEnd.ToString(), FrontEndWidth),
            Action = Clip(command.Name, ActionWidth),
            Target = target is null ? null : Clip(target, TargetWidth),
            Details = details.Count == 0 ? null : Clip(string.Join("; ", details), DetailsWidth),
            RemoteAddress = actor.RemoteAddress is null ? null : Clip(actor.RemoteAddress, RemoteAddressWidth),
            Outcome = reply.Outcome.ToString(),
        }));
        return reply;
    }

    // A value wider than its column makes the insert fail on every attempt, and the queue retries a failing insert for good.
    private static string Clip(string value, int width)
    {
        if (value.Length <= width)
            return value;

        // Cutting between the halves of a surrogate pair would leave a character the database cannot store.
        return char.IsHighSurrogate(value[width - 1]) ? value[..(width - 1)] : value[..width];
    }
}
