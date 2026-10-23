using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// One settler improvement's status in a hub, under the improvement's id.
/// </summary>
[WireStruct]
public sealed partial class SettlerImprovementStatus
{
    /// <summary>
    /// The improvement, by the id the hub files it under.
    /// </summary>
    [UInt(14)] public ushort ImprovementId { get; set; }

    /// <summary>
    /// The improvement's current tier, which SettlerImprovement:GetCurrentTier returns.
    /// </summary>
    [UInt(32)] public uint CurrentTier { get; set; }

    /// <summary>
    /// The improvement's remaining time, which SettlerImprovement:GetRemainingTime returns.
    /// </summary>
    [UInt(32)] public uint RemainingTime { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
