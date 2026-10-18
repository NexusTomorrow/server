using System.Collections.Immutable;
using System.Text;

namespace NexusTomorrow.Commands;

/// <summary>
/// A command's actor and its arguments, already checked against the command's parameters.
/// </summary>
public sealed record CommandInvocation(CommandActor Actor, IReadOnlyList<string> Arguments);

/// <summary>
/// A typed admin command: its name (one or two words), its parameters, the one permission key it needs and how it runs. The command
/// runs by posting to the owner of what it changes.
/// </summary>
public sealed class CommandDefinition
{
    /// <summary>
    /// The words that select the command, such as <c>account create</c>.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// What the command does, for help.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The permission key an actor must hold, or null when every actor may run it.
    /// </summary>
    public required string? Permission { get; init; }

    /// <summary>
    /// The parameters in order.
    /// </summary>
    public required ImmutableArray<CommandParameter> Parameters { get; init; }

    /// <summary>
    /// Whether the command refuses an actor who came in game.
    /// </summary>
    public bool ConsoleOnly { get; init; }

    /// <summary>
    /// Run the command once the dispatcher has checked its permission and arguments.
    /// </summary>
    public required Func<CommandInvocation, CancellationToken, Task<CommandReply>> ExecuteAsync { get; init; }

    /// <summary>
    /// The usage line, such as <c>account suspend &lt;email&gt; &lt;hours&gt; [reason...]</c>: a required parameter in angle brackets,
    /// an optional one in square brackets, the rest parameter with an ellipsis. With <paramref name="markSecrets"/> a secret parameter
    /// reads <c>&lt;password, secret&gt;</c>.
    /// </summary>
    public string Usage(bool markSecrets = false)
    {
        var usage = new StringBuilder(Name);
        foreach (CommandParameter parameter in Parameters)
        {
            string name = parameter.Rest ? $"{parameter.Name}..." : parameter.Name;
            if (markSecrets && parameter.Secret)
                name += ", secret";
            usage.Append(' ').Append(parameter.Optional ? $"[{name}]" : $"<{name}>");
        }

        return usage.ToString();
    }
}
