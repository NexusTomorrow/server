using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Starts a scientist experimentation at a unit, with the pattern to match.
/// </summary>
/// <remarks>
/// The client takes it only while the player is interacting as <see cref="InteractionType.ScientistActivate"/>. It finds the
/// experimentation mission (a PathMission row of type 22) for the unit's creature, keeps the pattern, fills its set of choices to four and
/// shuffles it, and raises InvokeScientistExperimentation. <see cref="ScientistExperimentationAttempt"/> carries an attempt that matches
/// the pattern.
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
