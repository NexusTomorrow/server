using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Adds a visual effect on a unit or at a position, with its sounds, at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server021C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicVisualEffectAdd
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The handle the client keeps the effect by, which <see cref="CinematicVisualEffectRemove"/> names it with.
    /// </summary>
    [UInt(32)] public uint EffectHandle { get; set; }

    /// <summary>
    /// The effect, a VisualEffect id.
    /// </summary>
    [UInt(17)] public uint VisualEffectId { get; set; }

    /// <summary>
    /// The unit the effect plays on, by unit id; 0 for none.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Where the effect plays when <see cref="UnitId"/> is 0.
    /// </summary>
    public Client0096Part0 Position { get; set; } = new();

    /// <summary>
    /// Removes the effect when the cinematic camera ends.
    /// </summary>
    [UInt(1)] public bool EndsWithCamera { get; set; }
}
