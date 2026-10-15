using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipIgnoreStrangersState)]
[WireMessage(GameMessageOpcode.ClientFriendshipIgnoreStrangersState, Direction.ClientToServer)]
public sealed partial class ClientFriendshipIgnoreStrangersState : IReadable
{
    [UInt(1)] public bool IgnoreStrangerInvites { get; private set; }

    public void Read(GamePacketReader reader)
    {
        IgnoreStrangerInvites = reader.ReadBit();
    }
}
