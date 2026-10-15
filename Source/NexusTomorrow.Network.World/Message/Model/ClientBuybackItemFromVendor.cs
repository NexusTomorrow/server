using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientBuybackItemFromVendor, Direction.ClientToServer)]
public sealed partial class ClientBuybackItemFromVendor
{
    [UInt(32)] public uint UniqueId { get; private set; }
}
