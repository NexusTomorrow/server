using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NexusTomorrow.Commands;

namespace NexusTomorrow.Server;

/// <summary>
/// Reads command lines from standard input and runs each as the actor <c>console</c>, which holds every permission, printing each
/// reply. It does nothing when <see cref="CliOptions.Enabled"/> is false or standard input is redirected, so a service manager or a
/// test never waits on a console nobody types at.
/// </summary>
public sealed class ConsoleFrontEnd : BackgroundService
{
    private static readonly CommandActor s_actor = new("console", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);

    private readonly CommandDispatcher _dispatcher;
    private readonly bool _enabled;
    private readonly TextReader? _input;
    private readonly TextWriter _output;

    /// <summary>
    /// Create the front end over the process's console.
    /// </summary>
    public ConsoleFrontEnd(CommandDispatcher dispatcher, IOptions<CliOptions> options)
        : this(dispatcher, options.Value.Enabled, Console.IsInputRedirected ? null : Console.In, Console.Out)
    {
    }

    // A null input is a redirected console: the front end stays off.
    internal ConsoleFrontEnd(CommandDispatcher dispatcher, bool enabled, TextReader? input, TextWriter output)
    {
        _dispatcher = dispatcher;
        _enabled = enabled;
        _input = input;
        _output = output;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_enabled || _input is null)
            return;

        try
        {
            while (await ReadLineAsync(_input, stoppingToken) is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                CommandReply reply = await _dispatcher.DispatchAsync(s_actor, line, stoppingToken);
                await _output.WriteLineAsync(reply.Text.TrimEnd('\n'));
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    // A console read cannot be cancelled once blocked, so it runs on a pool thread and the wait for it gives way to the host's stop.
    // The abandoned read ends with the process.
    private static Task<string?> ReadLineAsync(TextReader input, CancellationToken cancellationToken) =>
        Task.Run(input.ReadLine, CancellationToken.None).WaitAsync(cancellationToken);
}
