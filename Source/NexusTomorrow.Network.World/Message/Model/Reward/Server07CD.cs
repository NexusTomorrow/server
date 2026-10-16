using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07CD, Direction.ServerToClient)]
public sealed partial class Server07CD
{
    public Server07CDPart0 Unknown0 { get; set; }
}
