using System.Diagnostics.CodeAnalysis;
using System.Net;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Testing;

public sealed class FakeClientSession : IWorldSession
{
    private static long s_nextId;
    private readonly List<object> _sent = [];
    private readonly Lock _gate = new();
    private string? _disconnectReason;
    private IMapInbox? _map;

    public long Id { get; } = Interlocked.Increment(ref s_nextId);

    public IPAddress RemoteAddress { get; } = IPAddress.Loopback;

    public IReadOnlyList<object> Sent
    {
        get
        {
            lock (_gate)
                return [.. _sent];
        }
    }

    public string? DisconnectReason
    {
        get
        {
            lock (_gate)
                return _disconnectReason;
        }
    }

    // The map a map routed this session to; null before TryEnterMap and after LeaveMap.
    public IMapInbox? Map
    {
        get
        {
            lock (_gate)
                return _map;
        }
    }

    // When set, TryEnterMap throws it: a session fault for the owner's failure policy.
    public Exception? EnterMapFailure { get; set; }

    // Runs the server's own send gate, so an owner's test fails exactly where the connection would refuse the message.
    public void Send<T>(T message)
        where T : class, IWireMessage
    {
        if (!MessageCatalog.Default.MaySend(message, out string? refusal))
            throw new InvalidOperationException(refusal);

        lock (_gate)
            _sent.Add(message);
    }

    public void Disconnect(string reason)
    {
        lock (_gate)
            _disconnectReason ??= reason;
    }

    public bool TryEnterMap(IMapInbox map)
    {
        if (EnterMapFailure is { } failure)
            throw failure;

        lock (_gate)
        {
            if (_disconnectReason is not null)
                return false;

            _map = map;
            return true;
        }
    }

    public void LeaveMap(IMapInbox map)
    {
        lock (_gate)
        {
            if (ReferenceEquals(_map, map))
                _map = null;
        }
    }

    [SuppressMessage("Naming", "CA1720", Justification = "Single reads as LINQ's Single: the one sent message of a type, and it throws otherwise.")]
    public T Single<T>()
        where T : class, IWireMessage => ExactlyOne(Sent.OfType<T>().ToList());

    private static T ExactlyOne<T>(List<T> found) =>
        found.Count == 1 ? found[0] : throw new InvalidOperationException($"Expected one {typeof(T).Name}, found {found.Count}.");
}
