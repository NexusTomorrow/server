using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerVehiclePassengerRemove)]
[WireMessage(GameMessageOpcode.ServerVehiclePassengerRemove, Direction.ServerToClient)]
public sealed partial class ServerVehiclePassengerRemove : IWritable
{
    [UInt(32)] public uint Self { get; set; }
    [UInt(32)] public uint Passenger { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Self);
        writer.Write(Passenger);
    }
}
