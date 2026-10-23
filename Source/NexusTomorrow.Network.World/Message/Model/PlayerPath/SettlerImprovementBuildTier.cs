using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Builds a settler improvement at a tier.
/// </summary>
/// <remarks>
/// SettlerImprovement:BuildTier sends it, never past the improvement's tier count.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0670, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SettlerImprovementBuildTier
{
    /// <summary>
    /// The improvement, by its own id: one of <see cref="SettlerBuildInvoke.ImprovementIds"/>.
    /// </summary>
    [UInt(14)] public uint ImprovementId { get; private set; }

    /// <summary>
    /// The tier, counted from 0: one less than the tier SettlerImprovement:BuildTier is given.
    /// </summary>
    [UInt(32)] public uint Tier { get; private set; }
}
