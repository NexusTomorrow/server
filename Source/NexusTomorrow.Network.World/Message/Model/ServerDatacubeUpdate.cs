using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerDatacubeUpdate)]
[WireMessage(GameMessageOpcode.ServerDatacubeUpdate, Direction.ServerToClient)]
public sealed partial class ServerDatacubeUpdate : IWritable
{
    public Datacube DatacubeData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        DatacubeData.Write(writer);
    }
}
