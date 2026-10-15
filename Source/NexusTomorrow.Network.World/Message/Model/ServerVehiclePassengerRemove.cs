using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerVehiclePassengerRemove, Direction.ServerToClient)]
public sealed partial class ServerVehiclePassengerRemove
{
    [UInt(32)] public uint Self { get; set; }
    [UInt(32)] public uint Passenger { get; set; }
}
