using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07C7, Direction.ServerToClient)]
public sealed partial class Server07C7
{
    public Server07C7Part0 Unknown0 { get; set; } = new();
}
