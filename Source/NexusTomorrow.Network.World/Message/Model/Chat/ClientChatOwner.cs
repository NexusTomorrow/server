using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatOwner, Direction.ClientToServer)]
public sealed partial class ClientChatOwner
{
    public Channel Channel { get; private set; }
    [WideString] public string CharacterName { get; private set; }
}
