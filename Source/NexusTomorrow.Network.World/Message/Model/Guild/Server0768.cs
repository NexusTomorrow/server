using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0768, Direction.ServerToClient)]
public sealed partial class Server0768
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    public Server0768Part0 Unknown1 { get; set; } = new();
}

