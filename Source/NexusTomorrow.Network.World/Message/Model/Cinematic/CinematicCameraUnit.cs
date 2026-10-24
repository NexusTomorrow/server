using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Puts the cinematic camera's position, or the point it looks at, on a unit, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0217, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraUnit
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Sets the camera's look-at channel instead of its position channel.
    /// </summary>
    [UInt(1)] public bool LookAt { get; set; }
}
