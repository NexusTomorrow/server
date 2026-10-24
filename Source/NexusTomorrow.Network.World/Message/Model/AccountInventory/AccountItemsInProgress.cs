using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096B, Direction.ServerToClient)]
public sealed partial class AccountItemsInProgress
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<AccountItem> Unknown1 { get; set; } = [];
}
