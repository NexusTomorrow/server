using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets a group's loot rules.
/// </summary>
/// <remarks>
/// For its current group the client then raises Group_LootRulesChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server042F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupLootRulesChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The rule GroupLib.GetLootRules reports as eNormalRule.
    /// </summary>
    [UInt(3)] public byte NormalRule { get; set; }

    /// <summary>
    /// The rule GroupLib.GetLootRules reports as eThresholdRule.
    /// </summary>
    [UInt(3)] public byte ThresholdRule { get; set; }

    /// <summary>
    /// The quality GroupLib.GetLootRules reports as eThresholdQuality.
    /// </summary>
    [UInt(4)] public byte ThresholdQuality { get; set; }

    /// <summary>
    /// The rule GroupLib.GetLootRules reports as eHarvestRule.
    /// </summary>
    [UInt(2)] public byte HarvestRule { get; set; }
}
