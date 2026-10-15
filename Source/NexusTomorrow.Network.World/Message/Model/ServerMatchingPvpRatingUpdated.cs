using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingPvpRatingUpdated, Direction.ServerToClient)]
public sealed partial class ServerMatchingPvpRatingUpdated
{
    [UInt(32)] public uint Rating { get; set; }
    [UInt(32)] public uint Wins { get; set; }
    [UInt(32)] public uint Losses { get; set; }
    [UInt(32)] public uint Draws { get; set; }
    [UInt(3)] public MatchingGameRatingType Type { get; set; }
}
