using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server00CE, Direction.ServerToClient)]
public sealed partial class Server00CE
{
    [WideString] public string Unknown0 { get; set; } = "";
}
