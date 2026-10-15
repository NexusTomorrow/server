using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.ServerGenericMapShow, Direction.ServerToClient)]
public sealed partial class ServerGenericMapShow
{
    [UInt(14)] public uint GenericMapId { get; set; }
    [Array(CountBits = 32)] public List<GenericMapNode> Nodes { get; set; } = [];
}
