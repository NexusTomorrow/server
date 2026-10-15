using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountItemInProgressUpdate, Direction.ServerToClient)]
public sealed partial class ServerAccountItemInProgressUpdate
{
    [UInt(32)] public uint AccountId { get; set; } // is the AccountId sent ServerRealmInfo (0x3DB), filled by server but not used by client
    public AccountInventoryItem Item { get; set; }
}
