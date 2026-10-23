using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server058C, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server058C
{
    [WideString] public string Unknown0 { get; set; } = "";
}
