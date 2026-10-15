using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerPvpRatingUpdate, Direction.ServerToClient)]
public sealed partial class ServerPvpRatingUpdate
{
    [WireStruct]
    public sealed partial class PvpRating
    {
        [UInt(8)] public uint Category { get; set; } // Arena2v2, Arena3v3, Arena5v5, RatedBattleground = 5, Warplot = 1
        [UInt(3)] public MatchingGameRatingType Type { get; set; }
        [UInt(32)] public uint Rating { get; set; }
        [UInt(32)] public uint Wins { get; set; }
        [UInt(32)] public uint Losses { get; set; }
        [UInt(32)] public uint Draws { get; set; }
    }

    [Array(CountBits = 32)] public List<PvpRating> PvpRatings { get; set; } = [];
}
