using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Possibly for when an account friend deletes a character while the player is online
[Message(GameMessageOpcode.ServerFriendshipAccountRemovedCharacter)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountRemovedCharacter, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountRemovedCharacter : IWritable
{
    [UInt(32)] public uint AccountId { get; set; }
    public Identity CharacterRemoved { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        CharacterRemoved.Write(writer);
    }
}
