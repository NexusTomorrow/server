using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Adds a spell visual group to a unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server0264, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellVisualGroupAdded
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The visual group, a Spell4VisualGroup table id.
    /// </summary>
    [UInt(16)] public ushort Spell4VisualGroupId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown3 { get; set; } = [];
}
