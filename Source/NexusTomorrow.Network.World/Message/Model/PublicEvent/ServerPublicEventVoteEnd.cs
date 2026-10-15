using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventVoteEnd, Direction.ServerToClient)]
public sealed partial class ServerPublicEventVoteEnd
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(14)] public uint VoteId { get; set; }
    [UInt(32)] public uint Winner { get; set; }
}
