using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerDatacubeVolumeUpdate, Direction.ServerToClient)]
public sealed partial class ServerDatacubeVolumeUpdate
{
    public Datacube DatacubeVolumeData { get; set; }
}
