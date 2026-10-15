using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

// Only recorded use of this in sniffs is for using a character name change token.
// Only use of this in the client if for StoreAccountItemLib::IsRedeemCREDDInProgress that looks at the array
// of the AccountItemsInProgress items to see if a CREDD operation is underway. There are no sniffs recorded
// showing a CREDD account item operation listed as InProgress.
[WireMessage(GameMessageOpcode.ServerAccountItemsInProgress, Direction.ServerToClient)]
public sealed partial class ServerAccountItemsInProgress
{
    [UInt(32)] public uint AccountId { get; set; } // is the AccountId sent ServerRealmInfo (0x3DB), filled by server but not used by client
    [Array(CountBits = 32)] public List<AccountInventoryItem> Items { get; set; }
}
