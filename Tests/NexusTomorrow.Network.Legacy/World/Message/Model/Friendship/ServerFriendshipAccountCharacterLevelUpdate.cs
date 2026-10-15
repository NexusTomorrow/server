using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountCharacterLevelUpdate)]
public class ServerFriendshipAccountCharacterLevelUpdate : IWritable
{
    public uint AccountId { get; set; }
    public Identity Character { get; set; }
    public uint Level { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        Character.Write(writer);
        writer.Write(Level, 8u);
    }
}
