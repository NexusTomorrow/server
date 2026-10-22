using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets the group's loot rules: the request GroupLib.SetLootRules sends.
/// </summary>
/// <remarks>
/// The shipped add-ons call it with the four rules GroupLib.GetLootRules reports, in this order.
/// </remarks>
[WireMessage(GameMessageOpcode.Client042E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupSetLootRules
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

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
