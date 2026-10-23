using System.Threading.Channels;
using NexusTomorrow.Gateway.Connections;

namespace NexusTomorrow.Tools;

/// <summary>
/// One endpoint connection: the lines it is sent, and the taps it holds on sessions, detached when it ends.
/// </summary>
public sealed class OracleConnection : IDisposable
{
    private readonly ChannelWriter<string> _lines;
    private readonly Action? _full;
    private readonly List<(IDebugSession Session, ISessionTap Tap)> _taps = [];
    private readonly Lock _lock = new();

    /// <summary>
    /// Create a connection whose outgoing lines go to <paramref name="lines"/>; <paramref name="full"/> is called, from the sending
    /// thread, when a line does not fit.
    /// </summary>
    public OracleConnection(ChannelWriter<string> lines, Action? full = null)
    {
        _lines = lines;
        _full = full;
    }

    /// <summary>
    /// Send one line to the connection, from any thread; false, after calling the <c>full</c> callback, when the outbox is full or the
    /// connection has ended.
    /// </summary>
    public bool Send(string line)
    {
        if (_lines.TryWrite(line))
            return true;

        _full?.Invoke();
        return false;
    }

    /// <summary>
    /// Attach <paramref name="tap"/> to <paramref name="session"/> for as long as this connection lasts.
    /// </summary>
    public void Hold(IDebugSession session, ISessionTap tap)
    {
        lock (_lock)
            _taps.Add((session, tap));
        session.Tap = tap;
    }

    /// <summary>
    /// Detach every tap this connection attached that is still in place.
    /// </summary>
    public void Dispose()
    {
        lock (_lock)
        {
            foreach ((IDebugSession session, ISessionTap tap) in _taps)
                session.Untap(tap);

            _taps.Clear();
        }
    }
}
