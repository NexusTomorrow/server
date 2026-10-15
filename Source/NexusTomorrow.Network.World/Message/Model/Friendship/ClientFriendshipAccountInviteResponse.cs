using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountInviteResponse)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountInviteResponse : IReadable
{
    [UInt(64)] public ulong AccountFriendInviteId { get; private set; }
    [UInt(1)] public bool Response { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AccountFriendInviteId = reader.ReadULong();
        Response = reader.ReadBit();
    }
}
