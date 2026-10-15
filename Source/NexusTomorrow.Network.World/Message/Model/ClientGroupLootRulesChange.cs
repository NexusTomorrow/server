using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupLootRulesChange, Direction.ClientToServer)]
public sealed partial class ClientGroupLootRulesChange
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(3)] public LootRule LootRulesUnderThreshold { get; set; }

    [UInt(3)] public LootRule LootRulesThresholdAndOver { get; set; }

    [UInt(4)] public LootThreshold Threshold { get; set; }

    [UInt(2)] public HarvestLootRule HarvestingRule { get; set; }
}
