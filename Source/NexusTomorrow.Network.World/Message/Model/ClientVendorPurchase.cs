using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientVendorPurchase)]
[WireMessage(GameMessageOpcode.ClientVendorPurchase, Direction.ClientToServer)]
public sealed partial class ClientVendorPurchase : IReadable
{
    [UInt(32)] public uint VendorIndex { get; set; }
    [UInt(32)] public uint VendorItemQty { get; set; }

    public void Read(GamePacketReader reader)
    {
        VendorIndex = reader.ReadUInt();
        VendorItemQty = reader.ReadUInt();
    }
}
