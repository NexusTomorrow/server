using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPvpRatingUpdate)]
public class ServerPvpRatingUpdate : IWritable
{
    public class PvpRating : IWritable
    {
        public uint Category { get; set; } // Arena2v2, Arena3v3, Arena5v5, RatedBattleground = 5, Warplot = 1
        public MatchingGameRatingType Type { get; set; }
        public uint Rating { get; set; }
        public uint Wins { get; set; }
        public uint Losses { get; set; }
        public uint Draws { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Category, 8u);
            writer.Write(Type, 3u);
            writer.Write(Rating);
            writer.Write(Wins);
            writer.Write(Losses);
            writer.Write(Draws);
        }
    }

    public List<PvpRating> PvpRatings { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PvpRatings.Count);
        foreach (PvpRating rating in PvpRatings)
        {
            rating.Write(writer);
        }
    }
}
