using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatModerator)]
[WireMessage(GameMessageOpcode.ClientChatModerator, Direction.ClientToServer)]
public sealed partial class ClientChatModerator : IReadable
{
    public Channel Channel { get; private set; }
    [WideString] public string CharacterName { get; private set; }
    [UInt(1)] public bool MakeModerator { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Channel = new Channel();
        Channel.Read(reader);
        CharacterName = reader.ReadWideString();
        MakeModerator = reader.ReadBit();
    }
}
