using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One item a character or a unit shows in a slot.
/// </summary>
[WireStruct]
public sealed partial class ItemVisual
{
    /// <summary>
    /// The item slot, 0 to 71.
    /// </summary>
    [VerifiedMember, UInt(7)] public byte Slot { get; set; }

    /// <summary>
    /// The look to show in the slot, as an id in the ItemDisplay table.
    /// </summary>
    [VerifiedMember, UInt(15)] public ushort DisplayId { get; set; }

    /// <summary>
    /// A colour set, as an id in the ItemColorSet table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort ColorSetId { get; set; }

    /// <summary>
    /// The slot's colour: a top bit and three 10-bit channels below it, ramp indices which the client maps to DyeColorRamp rows by the
    /// table's ramp index. In the character list, 0 takes the whole colour from <see cref="ColorSetId"/>'s set and any other value is
    /// used as sent. On a unit, a colour with the top bit clear takes the whole colour from the set, or 0 when there is no set; with the
    /// top bit set, each channel left at 0 takes the set's when <see cref="ColorSetId"/> is not 0. On a unit, nothing reads bit 30.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Color { get; set; }
}
