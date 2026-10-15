using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAddByName)]
[WireMessage(GameMessageOpcode.ClientFriendshipAddByName, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAddByName : IReadable
{
    [WideString] public string Name { get; private set; }
    [WideString] public string RealmName { get; private set; }
    [UInt(4)] public FriendshipType Type { get; private set; }
    [WideString] public string Note { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Name = reader.ReadWideString();
        RealmName = reader.ReadWideString();
        Type = reader.ReadEnum<FriendshipType>(4);
        Note = reader.ReadWideString();
    }
}
