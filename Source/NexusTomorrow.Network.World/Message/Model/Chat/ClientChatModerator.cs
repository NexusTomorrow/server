using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatModerator, Direction.ClientToServer)]
public sealed partial class ClientChatModerator
{
    public Channel Channel { get; private set; }
    [WideString] public string CharacterName { get; private set; }
    [UInt(1)] public bool MakeModerator { get; private set; }
}
