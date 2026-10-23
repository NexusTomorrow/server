using System.Threading.Channels;
using NexusTomorrow.Gateway.Connections;

namespace NexusTomorrow.Tools;

/// <summary>
/// One endpoint connection: the lines it is sent, and the taps it holds on sessions, detached when it ends.
/// </summary>
public sealed class OracleConnection : IDisposable
{
    private readonly ChannelWriter<string> _lines;
    private readonly List<(IDebugSession Session, ISessionTap Tap)> _taps = [];
    private readonly Lock _lock = new();

    /// <summary>
    /// Create a connection whose outgoing lines go to <paramref name="lines"/>.
    /// </summary>
    public OracleConnection(ChannelWriter<string> lines)
    {
        _lines = lines;
    }

    /// <summary>
    /// Send one line to the connection, from any thread; false once the connection has ended.
    /// </summary>
    public bool Send(string line) => _lines.TryWrite(line);

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
            {
                if (session.Tap == tap)
                    session.Tap = null;
            }

            _taps.Clear();
        }
    }
}
