using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Message063E, Direction.Both)]
public sealed partial class Message063E
{
    [WideString] public string Unknown0 { get; set; } = "";
}
