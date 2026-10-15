using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerVehiclePassengerAdd, Direction.ServerToClient)]
public sealed partial class ServerVehiclePassengerAdd
{
    [UInt(32)] public uint Self { get; set; }
    [UInt(2)] public VehicleSeatType SeatType { get; set; }
    [UInt(3)] public byte SeatPosition { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
