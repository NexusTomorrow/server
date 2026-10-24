using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Pins the camera subject the client reports to a position: the client reports that position at once, with zero velocity, in
/// <see cref="CinematicCameraSubjectPositionVelocity"/>, and stops its per-frame subject report until the cinematic state resets.
/// </summary>
[WireMessage(GameMessageOpcode.Server0212, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraSubjectPin
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The position the client reports as the camera's subject.
    /// </summary>
    public Client0096Part0 Position { get; set; } = new();
}
