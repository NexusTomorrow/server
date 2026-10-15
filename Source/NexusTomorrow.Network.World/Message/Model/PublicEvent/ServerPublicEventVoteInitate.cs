using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Player automatically has the right to vote on the event when started this way.
[WireMessage(GameMessageOpcode.ServerPublicEventVoteInitate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventVoteInitate
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(14)] public uint VoteId { get; set; }
    [UInt(14)] public uint TeamId { get; set; }
}
