using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Pins the camera subject the client reports: the client sends <see cref="CinematicCameraSubjectPositionVelocity"/> at once and stops
/// its per-frame subject report until the cinematic state resets.
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

    public Client0096Part0 Unknown1 { get; set; } = new();
}
