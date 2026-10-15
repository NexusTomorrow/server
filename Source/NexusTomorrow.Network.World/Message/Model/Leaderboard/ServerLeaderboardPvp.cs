using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[Message(GameMessageOpcode.ServerLeaderboardPvp)]
[WireMessage(GameMessageOpcode.ServerLeaderboardPvp, Direction.ServerToClient)]
public sealed partial class ServerLeaderboardPvp : IWritable
{
    [UInt(4)] public LeaderboardType Type { get; set; }
    [UInt(64)] public ulong NextUpdateTime { get; set; } // Win32 FILETIME
    [Array(CountBits = 32)] public List<LeaderboardTeamPvp> Players { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 4u);
        writer.Write(NextUpdateTime);
        writer.Write(Players.Count);
        Players.ForEach(player => player.Write(writer));
    }
}
