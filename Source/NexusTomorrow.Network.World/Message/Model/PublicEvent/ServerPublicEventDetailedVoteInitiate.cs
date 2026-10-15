using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventDetailedVoteInitiate)]
[WireMessage(GameMessageOpcode.ServerPublicEventDetailedVoteInitiate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventDetailedVoteInitiate : IWritable
{
    [WireStruct]
    public sealed partial class Tally : IWritable
    {
        [UInt(32)] public uint Choice { get; set; }
        [UInt(32)] public uint Count { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Choice);
            writer.Write(Count);
        }
    }

    [UInt(14)] public uint EventId { get; set; }
    [UInt(14)] public uint VoteId { get; set; }
    [UInt(14)] public PublicEventTeam TeamId { get; set; }
    [Array(CountBits = 32)] public List<Tally> Tallies { get; set; } = [];
    [UInt(1)] public bool CanPlayerVote { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14);
        writer.Write(VoteId, 14);
        writer.Write(TeamId, 14);
        writer.Write(Tallies.Count);

        foreach (Tally tally in Tallies)
            tally.Write(writer);

        writer.Write(CanPlayerVote);
        writer.Write(ElapsedTimeMs);
    }
}
