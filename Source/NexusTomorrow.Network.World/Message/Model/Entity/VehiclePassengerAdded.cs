using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Seats a passenger in a vehicle.
/// </summary>
[WireMessage(GameMessageOpcode.Server086F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class VehiclePassengerAdded
{
    /// <summary>
    /// The vehicle, by unit id.
    /// </summary>
    [UInt(32)] public uint VehicleUnitId { get; set; }

    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(3)] public byte Unknown2 { get; set; }

    /// <summary>
    /// The passenger, by unit id.
    /// </summary>
    [UInt(32)] public uint PassengerUnitId { get; set; }
}
