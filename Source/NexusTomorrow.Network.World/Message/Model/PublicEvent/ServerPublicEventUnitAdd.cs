using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Only for adding a unit to a PublicEvent or PublicEventObjective. Ignores remove operations if specified.
[WireMessage(GameMessageOpcode.ServerPublicEventUnitAdd, Direction.ServerToClient)]
public sealed partial class ServerPublicEventUnitAdd
{
    [WireStruct]
    public sealed partial class PublicEventOperation
    {
        [UInt(32)] public uint ObjectId { get; set; } // PublicEventId or PublicEventObjectiveId depending on operation
        [UInt(3)] public PublicEventOperationType Operation { get; set; }
    }

    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 32)] public List<PublicEventOperation> Operations { get; set; } = [];
}
