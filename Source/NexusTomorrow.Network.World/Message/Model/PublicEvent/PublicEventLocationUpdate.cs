using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

/// <summary>
/// Updates the location of a public event.
/// </summary>
/// <remarks>
/// The client looks up the event and applies the location through one of four mode-specific setters, chosen by Mode.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0131, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PublicEventLocationUpdate
{
    /// <summary>
    /// The public event, by event id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint EventId { get; set; }

    /// <summary>
    /// The update mode, 0-3; each selects a different location aspect the client sets.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Mode { get; set; }

    /// <summary>
    /// The location value the client applies.
    /// </summary>
    [VerifiedMember, UInt(17)] public uint Location { get; set; }
}