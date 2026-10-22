using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client00BB, Direction.ClientToServer)]
public sealed partial class BuybackItemFromVendor
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
