using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Adds a full-screen effect at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server021A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicFullScreenEffectAdd
{
    /// <summary>
    /// The effect, a FullScreenEffect id.
    /// </summary>
    [UInt(32)] public uint FullScreenEffectId { get; set; }

    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }
}
