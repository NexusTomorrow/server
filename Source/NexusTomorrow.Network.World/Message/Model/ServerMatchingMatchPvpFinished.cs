using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchPvpFinished)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchPvpFinished, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchPvpFinished : IWritable
{
    [UInt(2)] public MatchWinner Winner { get; set; }
    [UInt(3)] public MatchEndReason Reason { get; set; }
    [UInt(32)] public uint RatingChangeTeam1 { get; set; }
    [UInt(32)] public uint RatingChangeTeam2 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Winner, 2u);
        writer.Write(Reason, 3u);
        writer.Write(RatingChangeTeam1);
        writer.Write(RatingChangeTeam2);
    }
}
