using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server00E1, Direction.ServerToClient)]
public sealed partial class Server00E1
{
    public Server00E0Part0 Unknown0 { get; set; } = new();
}
