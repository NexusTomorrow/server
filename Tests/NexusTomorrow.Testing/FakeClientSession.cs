using System.Diagnostics.CodeAnalysis;
using System.Net;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Owners;

namespace NexusTomorrow.Testing;

public sealed class FakeClientSession : IClientSession
{
    private static long s_nextId;
    private readonly List<object> _sent = [];
    private readonly Lock _gate = new();

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

    public string? DisconnectReason { get; private set; }

    public void Send<T>(T message)
        where T : class, IWireMessage
    {
        lock (_gate)
            _sent.Add(message);
    }

    public void Disconnect(string reason) => DisconnectReason ??= reason;

    [SuppressMessage("Naming", "CA1720", Justification = "Single reads as LINQ's Single: the one sent message of a type, and it throws otherwise.")]
    public T Single<T>()
        where T : class, IWireMessage => ExactlyOne(Sent.OfType<T>().ToList());

    private static T ExactlyOne<T>(List<T> found) =>
        found.Count == 1 ? found[0] : throw new InvalidOperationException($"Expected one {typeof(T).Name}, found {found.Count}.");
}
