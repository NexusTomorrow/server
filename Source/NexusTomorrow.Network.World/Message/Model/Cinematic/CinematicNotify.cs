using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Announces a cinematic, or queues the end of the one playing.
/// </summary>
/// <remarks>
/// With <see cref="CinematicFlags.Begin"/> the client resets its cinematic state, keeps the announcement and raises CinematicsNotify, which
/// offers the player the cinematic; with <see cref="CinematicFlags.PlayWithoutPrompt"/> as well, it plays the cinematic at once instead and
/// reports <see cref="CinematicState.Accepted"/> in <see cref="CinematicProgress"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0232, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicNotify
{
    /// <summary>
    /// How the client takes the announcement: <see cref="CinematicFlags.Begin"/> begins a cinematic, and without it the client queues the
    /// cinematic's end at <see cref="DurationMs"/>; <see cref="CinematicFlags.PlayWithoutPrompt"/> plays it without the prompt. The client
    /// never reads <see cref="CinematicFlags.Unknown2"/>.
    /// </summary>
    [UInt(16)] public CinematicFlags Flags { get; set; }

    /// <summary>
    /// What Escape, or a change of world, does while the cinematic plays: <see cref="CancelType.EndTransition"/> ends the camera through its
    /// end transition, and <see cref="CancelType.EndImmediate"/> ends the cinematic.
    /// </summary>
    [UInt(16)] public CancelType SkipMode { get; set; }

    /// <summary>
    /// How long the cinematic plays, in milliseconds: playback ends once it has passed.
    /// </summary>
    [UInt(32)] public uint DurationMs { get; set; }

    /// <summary>
    /// The cinematic, a CinematicRace cinematicId, which the client matches together with the unit's race.
    /// </summary>
    [UInt(14)] public ushort CinematicId { get; set; }
}
