using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerDatacubeUpdate, Direction.ServerToClient)]
public sealed partial class ServerDatacubeUpdate
{
    public Datacube DatacubeData { get; set; }
}
