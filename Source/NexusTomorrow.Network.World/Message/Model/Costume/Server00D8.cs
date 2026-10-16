using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Server00D8, Direction.ServerToClient)]
public sealed partial class Server00D8
{
    public Server00D8Part0 Unknown0 { get; set; } = new();
}
