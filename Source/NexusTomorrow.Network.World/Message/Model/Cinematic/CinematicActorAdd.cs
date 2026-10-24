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

    [UInt(16)] public ushort Unknown1 { get; set; }
    [SInt(16)] public short Unknown2 { get; set; }

    /// <summary>
    /// The actor's unit id, by which the later actor commands name it.
    /// </summary>
    [UInt(32)] public uint ActorUnitId { get; set; }

    /// <summary>
    /// The actor's creature, a Creature2 id, while bit 0 of <see cref="Unknown1"/> is clear. With that bit set the client makes the actor
    /// a copy of the player and does not use this value.
    /// </summary>
    [UInt(32)] public uint CreatureId { get; set; }

    [UInt(32)] public uint Unknown5 { get; set; }
    public Client0096Part0 Unknown6 { get; set; } = new();
    [UInt(64)] public ulong Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
}
