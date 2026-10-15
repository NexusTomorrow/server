using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientBuybackItemFromVendor)]
[WireMessage(GameMessageOpcode.ClientBuybackItemFromVendor, Direction.ClientToServer)]
public sealed partial class ClientBuybackItemFromVendor : IReadable
{
    [UInt(32)] public uint UniqueId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UniqueId = reader.ReadUInt();
    }
}
