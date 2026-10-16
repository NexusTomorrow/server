using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0487, Direction.ServerToClient)]
public sealed partial class Server0487
{
    public Server0487Part0 Unknown0 { get; set; }
}
