using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPvpRatingUpdate)]
[WireMessage(GameMessageOpcode.ServerPvpRatingUpdate, Direction.ServerToClient)]
public sealed partial class ServerPvpRatingUpdate : IWritable
{
    [WireStruct]
    public sealed partial class PvpRating : IWritable
    {
        [UInt(8)] public uint Category { get; set; } // Arena2v2, Arena3v3, Arena5v5, RatedBattleground = 5, Warplot = 1
        [UInt(3)] public MatchingGameRatingType Type { get; set; }
        [UInt(32)] public uint Rating { get; set; }
        [UInt(32)] public uint Wins { get; set; }
        [UInt(32)] public uint Losses { get; set; }
        [UInt(32)] public uint Draws { get; set; }

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

    [Array(CountBits = 32)] public List<PvpRating> PvpRatings { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PvpRatings.Count);
        foreach (PvpRating rating in PvpRatings)
        {
            rating.Write(writer);
        }
    }
}
