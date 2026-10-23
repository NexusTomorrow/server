using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Starts a scientist experimentation at a unit, with the pattern to match.
/// </summary>
/// <remarks>
/// The client finds the experimentation mission (a PathMission row of type 22) for the unit's creature, keeps the pattern, offers its
/// values shuffled as the four choices, and raises InvokeScientistExperimentation. It scores
/// <see cref="ScientistExperimentationAttempt"/> against the pattern.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0667, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ScientistExperimentationStarted
{
    /// <summary>
    /// The unit experimented on, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The pattern to match, four values in order.
    /// </summary>
    [FixedArray(4), UInt(32)] public uint[] Pattern { get; set; } = new uint[4];
}
