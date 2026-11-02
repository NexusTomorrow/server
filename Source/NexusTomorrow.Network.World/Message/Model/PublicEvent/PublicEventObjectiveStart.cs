using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

/// <summary>
/// Starts a public event objective; the client looks the objective up and raises PublicEventObjectiveStartMessage for it.
/// </summary>
[WireMessage(GameMessageOpcode.Server06F9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PublicEventObjectiveStart
{
    /// <summary>
    /// The public event objective, by id; the client looks it up in its public event store.
    /// </summary>
    [VerifiedMember, UInt(15)] public uint ObjectiveId { get; set; }
}
