using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientVendorPurchase, Direction.ClientToServer)]
public sealed partial class ClientVendorPurchase
{
    [UInt(32)] public uint VendorIndex { get; set; }
    [UInt(32)] public uint VendorItemQty { get; set; }
}
