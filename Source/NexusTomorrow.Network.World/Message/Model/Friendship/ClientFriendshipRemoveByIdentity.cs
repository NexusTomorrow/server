using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipRemoveByIdentity)]
[WireMessage(GameMessageOpcode.ClientFriendshipRemoveByIdentity, Direction.ClientToServer)]
public sealed partial class ClientFriendshipRemoveByIdentity : IReadable
{
    public Identity PlayerIdentity { get; private set; } = new();
    [UInt(4)] public FriendshipType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PlayerIdentity.Read(reader);
        Type = reader.ReadEnum<FriendshipType>(4u);
    }
}
