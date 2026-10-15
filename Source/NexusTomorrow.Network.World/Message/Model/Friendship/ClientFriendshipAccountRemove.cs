using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountRemove)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountRemove, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountRemove : IReadable
{
    [UInt(64)] public ulong AccountFriendId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AccountFriendId = reader.ReadULong();
    }
}
