using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatPassword, Direction.ClientToServer)]
public sealed partial class ClientChatPassword
{
    public Channel Channel { get; private set; }
    [WideString] public string Password { get; private set; }
}
