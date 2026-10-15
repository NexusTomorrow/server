using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventVoteEnd)]
[WireMessage(GameMessageOpcode.ServerPublicEventVoteEnd, Direction.ServerToClient)]
public sealed partial class ServerPublicEventVoteEnd : IWritable
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(14)] public uint VoteId { get; set; }
    [UInt(32)] public uint Winner { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14);
        writer.Write(VoteId, 14);
        writer.Write(Winner);
    }
}
