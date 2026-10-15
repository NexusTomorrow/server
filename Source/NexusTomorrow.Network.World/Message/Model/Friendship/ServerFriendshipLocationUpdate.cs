using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// One count, then every FriendshipId, then every WorldZoneId: element i of each list is one friend's location.
[Message(GameMessageOpcode.ServerFriendshipLocationUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipLocationUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipLocationUpdate : IWritable
{
    [UInt(8), Count] public byte FriendLocationCount { get; set; }
    [Array(CountFrom = nameof(FriendLocationCount)), UInt(64)] public List<ulong> FriendshipIds { get; set; } = [];
    [Array(CountFrom = nameof(FriendLocationCount)), UInt(32)] public List<uint> WorldZoneIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FriendshipIds.Count, 8);
        foreach (ulong friendshipId in FriendshipIds)
            writer.Write(friendshipId);

        foreach (uint worldZoneId in WorldZoneIds)
            writer.Write(worldZoneId, 32u);
    }
}
