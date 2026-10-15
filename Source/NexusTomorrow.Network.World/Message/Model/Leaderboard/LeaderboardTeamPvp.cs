using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireStruct]
public sealed partial class LeaderboardTeamPvp : IWritable
{
    [UInt(64)] public ulong GuildId { get; set; }
    [UInt(5)] public Class Class { get; set; } // value is 23 when leaderboard type is Arena3v3
    [UInt(32)] public uint Rating { get; set; }
    [UInt(32)] public uint Rank { get; set; }
    [UInt(32)] public uint LastRank { get; set; }
    [WideString] public string Name { get; set; } // can be team name or player name, depending on the leaderboard type
    [Array(CountBits = 32)] public List<TeamMember> TeamMembers { get; set; } = []; // Battleground rankings are for individuals only, there are no team members

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GuildId);
        writer.Write(Class, 5u);
        writer.Write(Rating);
        writer.Write(Rank);
        writer.Write(LastRank);
        writer.WriteStringWide(Name);
        writer.Write(TeamMembers.Count);
        TeamMembers.ForEach(member => member.Write(writer));
    }
}
