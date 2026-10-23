using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Adds a spell visual to a unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server0263, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellVisualAdded
{
    /// <summary>
    /// The visual's id, which removes it.
    /// </summary>
    [UInt(32)] public uint VisualId { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The visual effect, a VisualEffect table id.
    /// </summary>
    [UInt(17)] public uint VisualEffectId { get; set; }

    [UInt(17)] public uint Unknown3 { get; set; }

    /// <summary>
    /// The spell visual, a Spell4Visual table id.
    /// </summary>
    [UInt(17)] public uint Spell4VisualId { get; set; }

    [UInt(32)] public uint Unknown5 { get; set; }
    public Client0096Part0 Unknown6 { get; set; } = new();
}
