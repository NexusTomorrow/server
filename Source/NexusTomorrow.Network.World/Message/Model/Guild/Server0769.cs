using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0769, Direction.ServerToClient)]
public sealed partial class Server0769
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [WideString] public string Unknown1 { get; set; } = "";
}
