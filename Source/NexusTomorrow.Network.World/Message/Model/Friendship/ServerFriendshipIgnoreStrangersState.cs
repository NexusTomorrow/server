using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipIgnoreStrangersState)]
public class ServerFriendshipIgnoreStrangersState : IWritable
{
    public uint Flags { get; set; } // Only useful value is 2 = sets IgnoreStrangerInvites state on client

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Flags);
    }
}
