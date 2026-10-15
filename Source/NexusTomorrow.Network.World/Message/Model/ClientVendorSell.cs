using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientSellItemToVendor)]
[WireMessage(GameMessageOpcode.ClientSellItemToVendor, Direction.ClientToServer)]
public sealed partial class ClientVendorSell : IReadable
{
    public ItemLocation ItemLocation { get; } = new();
    [UInt(32)] public uint Quantity { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemLocation.Read(reader);
        Quantity = reader.ReadUInt();
    }
}
