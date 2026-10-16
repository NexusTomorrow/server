using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0737, Direction.ServerToClient)]
public sealed partial class Server0737
{
    public Server0737Part0 Unknown0 { get; set; } = new();
}
