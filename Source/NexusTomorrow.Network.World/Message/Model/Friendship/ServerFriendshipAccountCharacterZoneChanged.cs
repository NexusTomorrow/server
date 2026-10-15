using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountCharacterZoneChanged, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountCharacterZoneChanged
{
    [WireStruct]
    public sealed partial class FriendCharacterZoneInfo
    {
        [UInt(64)] public ulong AccountFriendId { get; set; }
        public Identity CharacterInNewZone { get; set; }
        [UInt(15)] public uint WorldZoneId { get; set; }
    }

    [Array(CountBits = 16)] public List<FriendCharacterZoneInfo> ZoneInfos { get; set; }
}
