using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientStorefrontRequestCatalog)]
[WireMessage(GameMessageOpcode.ClientStorefrontRequestCatalog, Direction.ClientToServer)]
public sealed partial class ClientStorefrontRequestCatalog : IReadable
{
    [UInt(14)] public ushort Unknown0 { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Unknown0 = reader.ReadUShort(14u);
    }
}
