using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Replaces a unit's bone values.
/// </summary>
/// <remarks>
/// The client applies them to the unit's model while the unit shows its own look (<see cref="UnitVisualsChanged.IsTemporaryAppearance"/>
/// clear), the costume preview takes them, and a cinematic copy of the unit carries them. It raises no event.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0931, Direction.ServerToClient)]
[Verified(Rung.Fields)]
[LayoutConfirmed]
public sealed partial class UnitBonesChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's bone values, which replace its own. While the unit shows its own look, the client applies them to its model with the
    /// bone set of its race and gender, whose count they must match; the costume preview takes them too.
    /// </summary>
    [Array(CountBits = 32), Float32] public List<float> BoneValues { get; set; } = [];
}
