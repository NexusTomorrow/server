using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Player automatically has the right to vote on the event when started this way.
[Message(GameMessageOpcode.ServerPublicEventVoteInitate)]
public class ServerPublicEventVoteInitate : IWritable
{
    public uint EventId { get; set; }
    public uint VoteId { get; set; }
    public uint TeamId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14);
        writer.Write(VoteId, 14);
        writer.Write(TeamId, 14);
    }
}
