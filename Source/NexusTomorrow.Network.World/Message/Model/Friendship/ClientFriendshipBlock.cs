using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipBlock)]
[WireMessage(GameMessageOpcode.ClientFriendshipBlock, Direction.ClientToServer)]
public sealed partial class ClientFriendshipBlock : IReadable
{
    [UInt(1)] public bool BlockFriendRequests { get; private set; }

    public void Read(GamePacketReader reader)
    {
        BlockFriendRequests = reader.ReadBit();
    }
}
