using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Fires whenever a group member selects an option during a public event vote.
[Message(GameMessageOpcode.ServerPublicEventVoteTally)]
[WireMessage(GameMessageOpcode.ServerPublicEventVoteTally, Direction.ServerToClient)]
public sealed partial class ServerPublicEventVoteTally : IWritable
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(14)] public uint VoteId { get; set; }
    [UInt(32)] public uint Choice { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14);
        writer.Write(VoteId, 14);
        writer.Write(Choice);
    }
}
