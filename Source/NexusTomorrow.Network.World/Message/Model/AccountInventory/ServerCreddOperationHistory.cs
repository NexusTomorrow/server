using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerCreddOperationHistory)]
[WireMessage(GameMessageOpcode.ServerCreddOperationHistory, Direction.ServerToClient)]
public sealed partial class ServerCreddOperationHistory : IWritable
{
    [WireStruct]
    public sealed partial class CreddOperationHistoryEntry : IWritable
    {
        [UInt(32)] public AccountOperation Operation { get; set; }
        [Pad(32)] public Pad OperationHigh { get; }
        [UInt(1)] public bool IsInitiator { get; set; }
        [UInt(32)] public uint LogAge { get; set; } // in minutes
        public Identity Transactor { get; set; } // is the character identity the player performed the transaction with
        public Identity Unused { get; set; }
        [UInt(64)] public ulong AccountFriendId { get; set; }
        [UInt(64)] public ulong MoneyAmount { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Operation);
            writer.Write(IsInitiator);
            writer.Write(LogAge);
            Transactor.Write(writer);
            Unused.Write(writer);
            writer.Write(AccountFriendId);
            writer.Write(MoneyAmount);
        }
    }

    [Array(CountBits = 32)] public List<CreddOperationHistoryEntry> HistoryEntries { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(HistoryEntries.Count);
        HistoryEntries.ForEach(entry => entry.Write(writer));
    }
}
