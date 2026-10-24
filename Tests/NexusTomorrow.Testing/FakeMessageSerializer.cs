using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Owners;

namespace NexusTomorrow.Testing;

// The server's serializer over the default catalog's gate, so an owner's test fails exactly where the server would, and a count of
// serializations by message type, so a test can pin that a message went out serialized once whatever the number of sessions.
public sealed class FakeMessageSerializer : IMessageSerializer
{
    private readonly GatedSerializer _serializer = new(MessageCatalog.Default);
    private readonly Dictionary<Type, int> _counts = [];
    private readonly Lock _gate = new();

    public SerializedMessage Serialize<T>(T message)
        where T : class, IWireMessage
    {
        SerializedMessage serialized = _serializer.Serialize(message);
        lock (_gate)
            _counts[serialized.MessageType] = _counts.GetValueOrDefault(serialized.MessageType) + 1;
        return serialized;
    }

    public int SerializedCount(Type messageType)
    {
        lock (_gate)
            return _counts.GetValueOrDefault(messageType);
    }
}
