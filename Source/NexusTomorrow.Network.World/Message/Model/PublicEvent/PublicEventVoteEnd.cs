using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

/// <summary>
/// Ends a public event vote.
/// </summary>
/// <remarks>
/// The client removes the matching vote record and raises PublicEventVoteEnded with the option id.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06FD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PublicEventVoteEnd
{
    /// <summary>
    /// The vote, by vote id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort VoteId { get; set; }

    /// <summary>
    /// The public event, by event id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort EventId { get; set; }

    /// <summary>
    /// The winning vote option, by option id, which the client passes to its PublicEventVoteEnded event.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint OptionId { get; set; }
}