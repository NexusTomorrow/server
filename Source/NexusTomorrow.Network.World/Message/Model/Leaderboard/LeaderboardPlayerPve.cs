using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireStruct]
public sealed partial class LeaderboardPlayerPve
{
    [UInt(64)] public ulong GuildId { get; set; }
    [UInt(5)] public Class Class { get; set; }
    [UInt(32)] public uint MatchingGameMapId { get; set; }
    [UInt(32)] public uint PrimeLevel { get; set; }
    [UInt(32)] public uint RewardedTier { get; set; }
    [UInt(32)] public uint CompletionTime { get; set; }
    [UInt(32)] public uint Rank { get; set; }
    [UInt(32)] public uint LastRank { get; set; }
    [WideString] public string Name { get; set; }
    [Array(CountBits = 32)] public List<TeamMember> TeamMembers { get; set; } = [];
}
