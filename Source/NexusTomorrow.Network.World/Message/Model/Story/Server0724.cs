using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server0724, Direction.ServerToClient)]
public sealed partial class Server0724
{
    [WideString] public string Unknown0 { get; set; } = "";
}
