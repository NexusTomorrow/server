using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.ClientPlayedRequest, Direction.ClientToServer)]
public sealed partial class ClientPlayedReqeust
{
}
