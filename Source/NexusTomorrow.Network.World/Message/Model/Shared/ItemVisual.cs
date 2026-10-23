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
    /// The look to show in the slot, as a display id.
    /// </summary>
    [VerifiedMember, UInt(15)] public ushort DisplayId { get; set; }

    /// <summary>
    /// A colour set, as an id in the ItemColorSet table, which the client looks up first. The set gives the slot's colour when
    /// <see cref="Color"/> is 0, and with the top bit of <see cref="Color"/> set it fills each channel left at 0.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort ColorSetId { get; set; }

    /// <summary>
    /// The slot's colour, with the top bit set and three 10-bit channels below it. A channel left at 0 takes its value from
    /// <see cref="ColorSetId"/>'s set, and 0 takes the whole colour from that set.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Color { get; set; }
}
