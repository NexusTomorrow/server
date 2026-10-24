using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Removes a visual effect at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0225, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicVisualEffectRemove
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The effect, by the <see cref="CinematicVisualEffectAdd.EffectHandle"/> that added it.
    /// </summary>
    [UInt(32)] public uint EffectHandle { get; set; }
}
