using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Sets which parts of the scene are drawn while the cinematic camera runs, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0227, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicSceneVisibility
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The parts of the scene kept drawn, one bit each: the client stops drawing the part of each clear bit. Which part each bit stands for
    /// is not yet named.
    /// </summary>
    [UInt(32)] public uint SceneMask { get; set; }
}
