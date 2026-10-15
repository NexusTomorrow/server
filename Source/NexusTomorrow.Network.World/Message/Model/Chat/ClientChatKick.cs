using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatKick, Direction.ClientToServer)]
public sealed partial class ClientChatKick
{
    public Channel Channel { get; private set; } = new();
    [WideString] public string CharacterName { get; private set; }
}
