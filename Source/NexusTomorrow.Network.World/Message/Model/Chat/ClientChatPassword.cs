using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatPassword)]
[WireMessage(GameMessageOpcode.ClientChatPassword, Direction.ClientToServer)]
public sealed partial class ClientChatPassword : IReadable
{
    public Channel Channel { get; private set; }
    [WideString] public string Password { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Channel = new Channel();
        Channel.Read(reader);
        Password = reader.ReadWideString();
    }
}
