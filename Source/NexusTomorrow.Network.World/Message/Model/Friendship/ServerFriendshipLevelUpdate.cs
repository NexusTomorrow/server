using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipLevelUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipLevelUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipLevelUpdate : IWritable
{
    public Identity Character { get; set; }
    [UInt(32)] public uint Level { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Character.Write(writer);
        writer.Write(Level);
    }
}
