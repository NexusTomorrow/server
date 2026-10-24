using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server00E0, Direction.ServerToClient)]
public sealed partial class DatacubeProgressList
{
    [Array(CountBits = 32)] public List<DatacubeProgress> Unknown0 { get; set; } = [];
    [Array(CountBits = 32)] public List<DatacubeProgress> Unknown1 { get; set; } = [];
}
