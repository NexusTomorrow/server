using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Removes a full-screen effect at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0223, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicFullScreenEffectRemove
{
    /// <summary>
    /// The effect, by the <see cref="CinematicFullScreenEffectAdd.FullScreenEffectId"/> that added it.
    /// </summary>
    [UInt(32)] public uint FullScreenEffectId { get; set; }

    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }
}
