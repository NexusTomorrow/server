using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Enters or leaves the client's cinematic interface mode at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server022E, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicInterfaceMode
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// Whether the client enters its cinematic interface mode or leaves it, the mode GameLib.UIStartCinematics and UIExitCinematics set.
    /// </summary>
    [UInt(1)] public bool Entered { get; set; }

    [UInt(1)] public bool Unknown2 { get; set; }
}
