using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipIgnoreStrangersState)]
public class ClientFriendshipIgnoreStrangersState : IReadable
{
    public bool IgnoreStrangerInvites { get; private set; }

    public void Read(GamePacketReader reader)
    {
        IgnoreStrangerInvites = reader.ReadBit();
    }
}
