using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One side's results in a war plot match.
/// </summary>
[WireStruct]
public sealed partial class WarPartyMatchResult
{
    /// <summary>
    /// The side's rating.
    /// </summary>
    [UInt(32)] public uint Rating { get; set; }

    /// <summary>
    /// What the side's repairs cost.
    /// </summary>
    [UInt(32)] public uint RepairCost { get; set; }

    /// <summary>
    /// How many of its plugs were destroyed.
    /// </summary>
    [UInt(32)] public uint DestroyedPlugs { get; set; }

    /// <summary>
    /// The war coins it earned.
    /// </summary>
    [UInt(32)] public uint WarCoinsEarned { get; set; }
}
