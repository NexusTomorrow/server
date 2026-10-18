using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0569, Direction.ServerToClient)]
public sealed partial class Server0569
{
    public Server0568Part0 Unknown0 { get; set; } = new();
}
