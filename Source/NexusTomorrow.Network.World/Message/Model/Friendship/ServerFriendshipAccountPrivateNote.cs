using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Can also be used to send the player's updated private note
[Message(GameMessageOpcode.ServerFriendshipAccountPrivateNote)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountPrivateNote, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountPrivateNote : IWritable
{
    [UInt(32)] public uint AccountId { get; set; }
    [WideString] public string PrivateNote { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        writer.WriteStringWide(PrivateNote);
    }
}
