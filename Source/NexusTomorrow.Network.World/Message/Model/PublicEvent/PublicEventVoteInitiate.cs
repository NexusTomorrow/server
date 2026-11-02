using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

/// <summary>
/// Initializes a public event vote.
/// </summary>
/// <remarks>
/// The client dedupes by the event and vote ids; a new vote loads the PublicEventVote table and records the option.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06F1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PublicEventVoteInitiate
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
    /// The vote option, by option id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort OptionId { get; set; }
}