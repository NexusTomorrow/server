using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0416, Direction.ClientToServer)]
public sealed partial class Client0416
{
    [WideString] public string Unknown0 { get; set; } = "";
    [WideString] public string Unknown1 { get; set; } = "";
}
