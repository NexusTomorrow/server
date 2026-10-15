using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipRemoveByName)]
[WireMessage(GameMessageOpcode.ClientFriendshipRemoveByName, Direction.ClientToServer)]
public sealed partial class ClientFriendshipRemoveByName : IReadable
{
    [WideString] public string Name { get; private set; }
    [WideString] public string RealmName { get; private set; }
    [UInt(4)] public FriendshipType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Name = reader.ReadWideString();
        RealmName = reader.ReadWideString();
        Type = reader.ReadEnum<FriendshipType>(4u);
    }
}
