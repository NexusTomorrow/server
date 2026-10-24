using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Sets the position the cinematic camera reports as its subject, and reports it at once in
/// <see cref="CinematicCameraSubjectPositionVelocity"/>, at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0212, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraMove
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The position, which the client reports with no velocity.
    /// </summary>
    public Client0096Part0 Position { get; set; } = new();
}
