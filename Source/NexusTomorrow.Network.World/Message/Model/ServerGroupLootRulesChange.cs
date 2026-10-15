using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupLootRulesChange)]
[WireMessage(GameMessageOpcode.ServerGroupLootRulesChange, Direction.ServerToClient)]
public sealed partial class ServerGroupLootRulesChange : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public uint UnknownDWord { get; set; }

    [UInt(3)] public LootRule LootRulesUnderThreshold { get; set; }

    [UInt(3)] public LootRule LootRulesThresholdAndOver { get; set; }

    [UInt(4)] public LootThreshold LootThreshold { get; set; }

    [UInt(2)] public HarvestLootRule HarvestLootRule { get; set; }


    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(UnknownDWord);
        writer.Write(LootRulesUnderThreshold, 3u);
        writer.Write(LootRulesThresholdAndOver, 3u);
        writer.Write(LootThreshold, 4u);
        writer.Write(HarvestLootRule, 2u);
    }
}
