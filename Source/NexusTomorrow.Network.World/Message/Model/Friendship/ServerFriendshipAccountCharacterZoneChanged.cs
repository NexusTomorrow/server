using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountCharacterZoneChanged)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountCharacterZoneChanged, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountCharacterZoneChanged : IWritable
{
    [WireStruct]
    public sealed partial class FriendCharacterZoneInfo : IWritable
    {
        [UInt(64)] public ulong AccountFriendId { get; set; }
        public Identity CharacterInNewZone { get; set; }
        [UInt(15)] public uint WorldZoneId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AccountFriendId);
            CharacterInNewZone.Write(writer);
            writer.Write(WorldZoneId, 15u);
        }
    }

    [Array(CountBits = 32)] public List<FriendCharacterZoneInfo> ZoneInfos { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ZoneInfos.Count);
        foreach (FriendCharacterZoneInfo zoneInfo in ZoneInfos)
            zoneInfo.Write(writer);
    }
}
