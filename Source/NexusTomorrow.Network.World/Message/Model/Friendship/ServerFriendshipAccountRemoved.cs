using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountRemoved)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountRemoved, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountRemoved : IWritable
{
    [UInt(64)] public ulong AccountFriendId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountFriendId);
    }
}
