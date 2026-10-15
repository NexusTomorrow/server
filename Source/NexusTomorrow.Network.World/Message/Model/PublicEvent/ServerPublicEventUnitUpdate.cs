using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Can add or remove a unit to a PublicEvent or PublicEventOjective.
[WireMessage(GameMessageOpcode.ServerPublicEventUnitUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventUnitUpdate
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint EventId { get; set; }
    [UInt(3)] public PublicEventOperationType Operation { get; set; }
}
