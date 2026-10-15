using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatList)]
[WireMessage(GameMessageOpcode.ClientChatList, Direction.ClientToServer)]
public sealed partial class ClientChatList : IReadable
{
    public Channel Channel { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Channel = new Channel();
        Channel.Read(reader);
    }
}
