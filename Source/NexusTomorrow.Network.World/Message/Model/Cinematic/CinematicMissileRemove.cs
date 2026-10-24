using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Removes a missile at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0224, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicMissileRemove
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The missile, by the <see cref="CinematicMissileAdd.MissileHandle"/> that launched it.
    /// </summary>
    [UInt(32)] public uint MissileHandle { get; set; }
}
