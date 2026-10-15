using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipIgnoreStrangersState)]
[WireMessage(GameMessageOpcode.ServerFriendshipIgnoreStrangersState, Direction.ServerToClient)]
public sealed partial class ServerFriendshipIgnoreStrangersState : IWritable
{
    [UInt(32)] public uint Flags { get; set; } // Only useful value is 2 = sets IgnoreStrangerInvites state on client

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Flags);
    }
}
