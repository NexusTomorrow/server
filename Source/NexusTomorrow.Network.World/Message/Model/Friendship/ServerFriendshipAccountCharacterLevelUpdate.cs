using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountCharacterLevelUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountCharacterLevelUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountCharacterLevelUpdate : IWritable
{
    [UInt(32)] public uint AccountId { get; set; }
    public Identity Character { get; set; }
    [UInt(8)] public uint Level { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        Character.Write(writer);
        writer.Write(Level, 8u);
    }
}
