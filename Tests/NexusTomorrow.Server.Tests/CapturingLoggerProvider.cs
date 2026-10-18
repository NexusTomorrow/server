using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Server.Tests;

// Keeps every formatted line, with its exception, from every logger it made, so a test can search them all.
internal sealed class CapturingLoggerProvider : ILoggerProvider
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

    public ILogger CreateLogger(string categoryName) => new Capture(this);

    public void Dispose()
    {
    }

    private void Add(string line)
    {
        lock (_gate)
            _lines.Add(line);
    }

    private sealed class Capture : ILogger
    {
        private readonly CapturingLoggerProvider _owner;

        public Capture(CapturingLoggerProvider owner)
        {
            _owner = owner;
        }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            _owner.Add($"{formatter(state, exception)} {exception}".TrimEnd());
    }
}
