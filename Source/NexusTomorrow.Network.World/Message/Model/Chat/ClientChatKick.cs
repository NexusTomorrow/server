using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatKick)]
[WireMessage(GameMessageOpcode.ClientChatKick, Direction.ClientToServer)]
public sealed partial class ClientChatKick : IReadable
{
    public Channel Channel { get; private set; } = new();
    [WideString] public string CharacterName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Channel.Read(reader);
        CharacterName = reader.ReadWideString();
    }
}
