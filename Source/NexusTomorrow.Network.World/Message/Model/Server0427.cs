using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0427, Direction.ServerToClient)]
public sealed partial class Server0427
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    public Server0427Part0 Unknown1 { get; set; } = new Server0427Part0();
}
