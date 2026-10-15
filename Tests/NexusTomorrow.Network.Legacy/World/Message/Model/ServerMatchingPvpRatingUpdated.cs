using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingPvpRatingUpdated)]
public class ServerMatchingPvpRatingUpdated : IWritable
{
    public uint Rating { get; set; }
    public uint Wins { get; set; }
    public uint Losses { get; set; }
    public uint Draws { get; set; }
    public MatchingGameRatingType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Rating);
        writer.Write(Wins);
        writer.Write(Losses);
        writer.Write(Draws);
        writer.Write(Type, 3u);
    }
}
