using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0568, Direction.ServerToClient)]
public sealed partial class Server0568
{
    public Server0568Part0 Unknown0 { get; set; } = new();
    public Server0568Part0 Unknown1 { get; set; } = new();
}
