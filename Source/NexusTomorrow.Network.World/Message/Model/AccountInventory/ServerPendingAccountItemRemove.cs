using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerPendingAccountItemRemove, Direction.ServerToClient)]
public sealed partial class ServerPendingAccountItemRemove
{
    [WideString] public string TransactionId { get; set; }
    [UInt(1)] public bool Redeemed { get; set; } // not used by client
}
