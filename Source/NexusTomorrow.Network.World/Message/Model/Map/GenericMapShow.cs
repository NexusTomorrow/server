using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0758, Direction.ServerToClient)]
public sealed partial class GenericMapShow
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<GenericMapNodeState> Unknown1 { get; set; } = [];
}
