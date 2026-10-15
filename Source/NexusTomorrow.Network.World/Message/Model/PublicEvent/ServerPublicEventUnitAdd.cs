using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Only for adding a unit to a PublicEvent or PublicEventObjective. Ignores remove operations if specified.
[Message(GameMessageOpcode.ServerPublicEventUnitAdd)]
public partial class ServerPublicEventUnitAdd : IWritable
{
    [WireStruct]
    public sealed partial class PublicEventOperation : IWritable
    {
        [UInt(32)] public uint ObjectId { get; set; } // PublicEventId or PublicEventObjectiveId depending on operation
        [UInt(3)] public PublicEventOperationType Operation { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ObjectId, 32u);
            writer.Write(Operation, 3u);
        }
    }

    public uint UnitId { get; set; }
    public List<PublicEventOperation> Operations { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        Operations.ForEach(operation => operation.Write(writer));
    }
}
