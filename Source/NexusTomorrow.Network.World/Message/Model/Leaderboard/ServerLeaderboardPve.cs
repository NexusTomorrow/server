using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[Message(GameMessageOpcode.ServerLeaderboardPve)]
[WireMessage(GameMessageOpcode.ServerLeaderboardPve, Direction.ServerToClient)]
public sealed partial class ServerLeaderboardPve : IWritable
{
    [UInt(4)] public LeaderboardType Type { get; set; }
    [UInt(32)] public uint MatchingGameMapId { get; set; }
    [UInt(32)] public uint PrimeLevel { get; set; }
    [UInt(64)] public ulong NextUpdateTime { get; set; } // Win32 FILETIME
    [Array(CountBits = 32)] public List<LeaderboardPlayerPve> Players { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 4u);
        writer.Write(MatchingGameMapId);
        writer.Write(PrimeLevel);
        writer.Write(NextUpdateTime);
        writer.Write(Players.Count);
        Players.ForEach(player => player.Write(writer));
    }
}
