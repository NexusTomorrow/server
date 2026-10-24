using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A unit that takes this unit as its vehicle, with its seat type and one other value.
/// </summary>
[WireStruct]
public sealed partial class Server0262Part0
{
    /// <summary>
    /// The kind of seat the unit takes on this vehicle: the client picks the unit's attachment point by it, and plays the passenger
    /// animations only for seat types 0 to 2.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte SeatType { get; set; }

    [UInt(3)] public byte Unknown1 { get; set; }

    /// <summary>
    /// A unit, by unit id. If the client has that unit, that unit takes this one as its vehicle, with the two values before this one.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }
}
