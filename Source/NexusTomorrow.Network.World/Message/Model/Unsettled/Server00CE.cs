using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server00CE, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server00CE
{
    [WideString] public string Unknown0 { get; set; } = "";
}
