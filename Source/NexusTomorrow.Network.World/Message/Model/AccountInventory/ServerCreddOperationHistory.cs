using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerCreddOperationHistory, Direction.ServerToClient)]
public sealed partial class ServerCreddOperationHistory
{
    [WireStruct]
    public sealed partial class CreddOperationHistoryEntry
    {
        [UInt(32)] public AccountOperation Operation { get; set; }
        [Pad(32)] public Pad OperationHigh { get; }
        [UInt(1)] public bool IsInitiator { get; set; }
        [UInt(32)] public uint LogAge { get; set; } // in minutes
        public Identity Transactor { get; set; } // is the character identity the player performed the transaction with
        public Identity Unused { get; set; }
        [UInt(64)] public ulong AccountFriendId { get; set; }
        [UInt(64)] public ulong MoneyAmount { get; set; }
    }

    [Array(CountBits = 32)] public List<CreddOperationHistoryEntry> HistoryEntries { get; set; } = [];
}
