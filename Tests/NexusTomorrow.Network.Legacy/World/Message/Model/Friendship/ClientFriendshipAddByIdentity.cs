using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountAddByIdentity)]
public class ClientFriendshipAddByIdentity : IWritable, IReadable
{
    public Identity Target { get; set; } = new(); // Match the account from the target's identity
    public FriendshipType Type { get; set; }
    public string Note { get; set; } // Optional note sent with invite

    public void Read(GamePacketReader reader)
    {
        Target.Read(reader);
        Type = reader.ReadEnum<FriendshipType>(4);
        Note = reader.ReadWideString();

    }

    public void Write(GamePacketWriter writer)
    {
        Target.Write(writer);
        writer.Write(Type, 4u);
        writer.WriteStringWide(Note);
    }
}
