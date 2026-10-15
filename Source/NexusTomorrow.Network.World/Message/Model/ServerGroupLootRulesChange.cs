using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupLootRulesChange, Direction.ServerToClient)]
public sealed partial class ServerGroupLootRulesChange
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public uint UnknownDWord { get; set; }

    [UInt(3)] public LootRule LootRulesUnderThreshold { get; set; }

    [UInt(3)] public LootRule LootRulesThresholdAndOver { get; set; }

    [UInt(4)] public LootThreshold LootThreshold { get; set; }

    [UInt(2)] public HarvestLootRule HarvestLootRule { get; set; }
}
