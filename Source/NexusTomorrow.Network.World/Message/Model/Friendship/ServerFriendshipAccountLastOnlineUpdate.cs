using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountLastOnlineUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountLastOnlineUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountLastOnlineUpdate : IWritable
{
    [UInt(32)] public uint AccountId { get; set; }
    [Float32] public float DaysSinceLastOnline { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        writer.Write(DaysSinceLastOnline);
    }
}
