using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipSetNote)]
[WireMessage(GameMessageOpcode.ServerFriendshipSetNote, Direction.ServerToClient)]
public sealed partial class ServerFriendshipSetNote : IWritable
{
    [UInt(64)] public ulong FriendshipId { get; set; }
    [WideString] public string Note { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FriendshipId);
        writer.WriteStringWide(Note);
    }
}
