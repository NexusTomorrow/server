using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Takes a passenger out of a vehicle.
/// </summary>
/// <remarks>
/// The client raises Mount for the passenger.
/// </remarks>
[WireMessage(GameMessageOpcode.Server08C7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class VehiclePassengerRemoved
{
    /// <summary>
    /// The vehicle, by unit id.
    /// </summary>
    [UInt(32)] public uint VehicleUnitId { get; set; }

    /// <summary>
    /// The passenger, by unit id.
    /// </summary>
    [UInt(32)] public uint PassengerUnitId { get; set; }
}
