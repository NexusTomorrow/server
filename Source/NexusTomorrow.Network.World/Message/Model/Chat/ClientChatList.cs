using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatList, Direction.ClientToServer)]
public sealed partial class ClientChatList
{
    public Channel Channel { get; private set; }
}
