using NexusTomorrow.Network.Codec;
using NexusTomorrow.Owners;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Catalog;

/// <summary>
/// The server's <see cref="IMessageSerializer"/>: the send gate (<see cref="MessageCatalog.MaySend"/>) once, then the body once into a
/// pooled buffer, which every session it goes to shares.
/// </summary>
public sealed class GatedSerializer : IMessageSerializer
{
    private readonly MessageCatalog _catalog;

    /// <summary>
    /// Create a serializer gated by <paramref name="catalog"/>.
    /// </summary>
    public GatedSerializer(MessageCatalog catalog)
    {
        _catalog = catalog;
    }

    /// <inheritdoc/>
    public SerializedMessage Serialize<T>(T message)
        where T : class, IWireMessage
    {
        if (!_catalog.MaySend(message, out string? refusal))
            throw new InvalidOperationException($"{refusal}, so the server may not send it.");

        byte[] body = FrameWriter.RentBody(message, out int length);
        return SerializedMessage.FromRentedBody(T.Opcode, message.GetType(), body, length);
    }
}
