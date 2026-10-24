using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Moves the cinematic camera's position, or the point it looks at, from a position at a constant velocity, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0214, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraPositionVelocity
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The position the camera's channel starts at.
    /// </summary>
    public Client0096Part0 Position { get; set; } = new();

    /// <summary>
    /// The velocity, per second, the client moves the channel's position by.
    /// </summary>
    [Float32] public Vector3 Velocity { get; set; }

    /// <summary>
    /// Sets the camera's look-at channel instead of its position channel.
    /// </summary>
    [UInt(1)] public bool LookAt { get; set; }
}
