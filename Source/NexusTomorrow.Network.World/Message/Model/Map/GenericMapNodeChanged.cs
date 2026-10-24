using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0737, Direction.ServerToClient)]
public sealed partial class GenericMapNodeChanged
{
    public GenericMapNodeState Unknown0 { get; set; } = new();
}
