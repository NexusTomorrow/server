using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventDetailedVoteInitiate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventDetailedVoteInitiate
{
    [WireStruct]
    public sealed partial class Tally
    {
        [UInt(32)] public uint Choice { get; set; }
        [UInt(32)] public uint Count { get; set; }
    }

    [UInt(14)] public uint EventId { get; set; }
    [UInt(14)] public uint VoteId { get; set; }
    [UInt(14)] public PublicEventTeam TeamId { get; set; }
    [Array(CountBits = 32)] public List<Tally> Tallies { get; set; } = [];
    [UInt(1)] public bool CanPlayerVote { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }
}
