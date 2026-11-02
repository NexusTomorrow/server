using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Seats a passenger in a vehicle.
/// </summary>
[WireMessage(GameMessageOpcode.Server086F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class VehiclePassengerAdded
{
    /// <summary>
    /// The vehicle, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint VehicleUnitId { get; set; }

    /// <summary>
    /// The passenger's seat index.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte SeatIndex { get; set; }

    /// <summary>
    /// The seat's flags.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Flags { get; set; }

    /// <summary>
    /// The passenger, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PassengerUnitId { get; set; }
}
