using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Commands.Tests;

// Keeps every entry, with its exception, so a test can search them all.
internal sealed class CapturingLogger<T> : ILogger<T>
{
    private readonly List<string> _lines = [];
    private readonly Lock _gate = new();

    public IReadOnlyList<string> Lines
    {
        get
        {
            lock (_gate)
                return [.. _lines];
        }
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_gate)
            _lines.Add($"{formatter(state, exception)} {exception}");
    }
}
