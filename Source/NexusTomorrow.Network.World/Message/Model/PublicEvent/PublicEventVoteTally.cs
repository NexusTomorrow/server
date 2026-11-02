using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

/// <summary>
/// Tallies a vote for an option in a public event.
/// </summary>
/// <remarks>
/// The client finds the event and its option, increments the option's tally, then raises PublicEventVoteTallied.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06FF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PublicEventVoteTally
{
    /// <summary>
    /// The public event, by event id.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint EventId { get; set; }

    /// <summary>
    /// The vote option, by option id.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint OptionId { get; set; }

    /// <summary>
    /// The tally count for the option, raised as the PublicEventVoteTallied event's argument.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Tally { get; set; }
}
