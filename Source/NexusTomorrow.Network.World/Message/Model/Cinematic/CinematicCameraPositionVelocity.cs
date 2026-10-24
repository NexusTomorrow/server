using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Moves the cinematic camera from the command's time.
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

    public Client0096Part0 Unknown1 { get; set; } = new();
    [Float32] public Vector3 Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}
