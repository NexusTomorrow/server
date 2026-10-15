using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.ServerLeaderboardPvp, Direction.ServerToClient)]
public sealed partial class ServerLeaderboardPvp
{
    [UInt(4)] public LeaderboardType Type { get; set; }
    [UInt(64)] public ulong NextUpdateTime { get; set; } // Win32 FILETIME
    [Array(CountBits = 32)] public List<LeaderboardTeamPvp> Players { get; set; } = [];
}
