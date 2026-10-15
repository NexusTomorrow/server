using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientCreddExchangeOrderCancel, Direction.ClientToServer)]
public sealed partial class ClientCreddExchangeOrderCancel
{
    public Identity Unused { get; private set; } = new();
    [UInt(64)] public ulong CreddOrderId { get; private set; }
}
