using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server00E1, Direction.ServerToClient)]
public sealed partial class DatacubeProgressUpdate
{
    public DatacubeProgress Unknown0 { get; set; } = new();
}
