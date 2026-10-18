using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client0253, Direction.ClientToServer)]
public sealed partial class Client0253
{
    [WideString] public string Unknown0 { get; set; } = "";
    [WideString] public string Unknown1 { get; set; } = "";
}
