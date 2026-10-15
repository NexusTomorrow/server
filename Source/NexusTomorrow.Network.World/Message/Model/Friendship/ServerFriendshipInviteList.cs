using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipInviteList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipInviteList
{
    [WireStruct]
    public sealed partial class InviteData
    {
        [UInt(64)] public ulong InviteId { get; set; }
        public Identity PlayerIdentity { get; set; }
        [UInt(3)] public uint Seen { get; set; }
        [Float32] public float ExpiryInDays { get; set; }
        [WideString] public string Note { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(8)] public Class Class { get; set; }
        [Pad(6)] public Pad ClassHigh { get; }
        [UInt(3)] public Game.Static.PlayerPath.Path Path { get; set; }
        [UInt(8)] public byte Level { get; set; }
    }

    [Array(CountBits = 16)] public List<InviteData> Invites { get; set; } = [];
}
