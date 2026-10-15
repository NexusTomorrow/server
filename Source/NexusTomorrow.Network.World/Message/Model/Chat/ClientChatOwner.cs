using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatOwner)]
[WireMessage(GameMessageOpcode.ClientChatOwner, Direction.ClientToServer)]
public sealed partial class ClientChatOwner : IReadable
{
    public Channel Channel { get; private set; }
    [WideString] public string CharacterName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Channel = new Channel();
        Channel.Read(reader);
        CharacterName = reader.ReadWideString();
    }
}
