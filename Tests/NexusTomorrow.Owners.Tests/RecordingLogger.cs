using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners.Tests;

// Keeps every entry as text, its level first, so a test can count and read what was logged.
internal sealed class RecordingLogger<T> : ILogger<T>
{
    private readonly Lock _gate = new();
    private readonly List<string> _entries = [];

    public IReadOnlyList<string> Entries
    {
        get
        {
            lock (_gate)
                return [.. _entries];
        }
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_gate)
            _entries.Add($"{logLevel} {eventId} {formatter(state, exception)} {exception}");
    }
}
