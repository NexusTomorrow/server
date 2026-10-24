using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Adds a cinematic actor, a unit the client makes for the cinematic, at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0228, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicActorAdd
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The actor's options: bit 0 makes the actor a copy of the player.
    /// </summary>
    [UInt(16)] public ushort Options { get; set; }

    /// <summary>
    /// An offset to the actor's height, which the client negates and adds to the model's height when it places the actor.
    /// </summary>
    [SInt(16)] public short HeightOffset { get; set; }

    /// <summary>
    /// The actor's unit id, by which the later actor commands name it.
    /// </summary>
    [UInt(32)] public uint ActorUnitId { get; set; }

    /// <summary>
    /// The actor's creature, a Creature2 id, which the client does not use when <see cref="Options"/> makes the actor a copy of the player.
    /// </summary>
    [UInt(32)] public uint CreatureId { get; set; }

    /// <summary>
    /// The actor's mode, the one <see cref="CinematicActorMode"/> changes.
    /// </summary>
    [UInt(32)] public uint Mode { get; set; }

    /// <summary>
    /// Where the client places the actor.
    /// </summary>
    public Client0096Part0 Position { get; set; } = new();

    [UInt(64)] public ulong Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
}
