using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatMute)]
[WireMessage(GameMessageOpcode.ClientChatMute, Direction.ClientToServer)]
public sealed partial class ClientChatMute : IReadable
{
    public Channel Channel { get; private set; }
    [WideString] public string CharacterName { get; private set; } // if left empty, mutes the entire channel
    [UInt(1)] public bool Muted { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Channel = new Channel();
        Channel.Read(reader);
        CharacterName = reader.ReadWideString();
        Muted = reader.ReadBit();
    }
}
