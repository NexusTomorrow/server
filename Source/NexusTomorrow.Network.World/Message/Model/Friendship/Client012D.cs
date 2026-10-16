using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client012D, Direction.ClientToServer)]
public sealed partial class Client012D
{
    [WideString] public string Unknown0 { get; set; } = "";
}
