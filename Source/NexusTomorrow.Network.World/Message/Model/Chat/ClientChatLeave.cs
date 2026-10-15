using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatLeave, Direction.ClientToServer)]
public sealed partial class ClientChatLeave
{
    public Channel Channel { get; private set; }
}
