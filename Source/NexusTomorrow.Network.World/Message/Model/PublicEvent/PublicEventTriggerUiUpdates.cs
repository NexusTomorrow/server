using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

/// <summary>
/// Signals a public event starting or clearing.
/// </summary>
/// <remarks>
/// The client looks up the event; a cleared flag raises PublicEventCleared, otherwise PublicEventStart.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0700, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PublicEventTriggerUiUpdates
{
    /// <summary>
    /// The public event, by event id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint EventId { get; set; }

    /// <summary>
    /// When clear, the event is being cleared (PublicEventCleared); when set, it is starting (PublicEventStart).
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Started { get; set; }
}