using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// One costume: its index and type, which slots show, and each slot's item and dyes.
/// </summary>
[WireStruct]
public sealed partial class CostumeEntry
{
    /// <summary>
    /// The costume's index among those of its type: 0 to 11 for the character's, 0 to 4 for a mannequin's. The client drops
    /// a costume whose index is out of range for its type.
    /// </summary>
    [UInt(32)] public uint Index { get; set; }

    /// <summary>
    /// Which slots show, bit i for <see cref="CostumeItemSlot"/> i, as Costume:IsSlotVisible reads it.
    /// </summary>
    [UInt(32)] public uint VisibleSlots { get; set; }

    /// <summary>
    /// Whether the costume is the character's or a mannequin's.
    /// </summary>
    [UInt(2)] public CostumeType Type { get; set; }

    /// <summary>
    /// The item in each slot, by Item2 id, indexed by <see cref="CostumeItemSlot"/>.
    /// </summary>
    [FixedArray(7), UInt(32)] public uint[] ItemIds { get; set; } = new uint[7];

    /// <summary>
    /// Each slot's dyes as one packed word: bit 31 is set when the slot has dyes, and bits 0 to 9, 10 to 19 and 20 to 29 are three ramp indices, which the client maps to DyeColorRamp rows by the table's ramp index.
    /// </summary>
    [FixedArray(7), UInt(32)] public uint[] Dyes { get; set; } = new uint[7];
}
