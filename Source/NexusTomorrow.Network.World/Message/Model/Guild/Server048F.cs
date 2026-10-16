using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server048F, Direction.ServerToClient)]
public sealed partial class Server048F
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [WideString] public string Unknown1 { get; set; } = "";
}
