using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipSetAutoResponseMessage)]
[WireMessage(GameMessageOpcode.ClientFriendshipSetAutoResponseMessage, Direction.ClientToServer)]
public sealed partial class ClientFriendshipSetAutoResponseMessage : IReadable
{
    [WideString] public string AwayMessage { get; private set; }
    [WideString] public string BusyMessage { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AwayMessage = reader.ReadWideString();
        BusyMessage = reader.ReadWideString();
    }
}
