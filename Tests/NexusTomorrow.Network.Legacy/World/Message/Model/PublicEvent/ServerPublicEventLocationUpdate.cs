using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventLocationUpdate)]
public class ServerPublicEventLocationUpdate : IWritable
{
    public uint ObjectId { get; set; } // Can be either PublicEventId or PublicEventObjectiveId depending on the operation
    public PublicEventOperationType Operation { get; set; }
    public uint WorldLocation2Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectId, 14u);
        writer.Write(Operation, 3u);
        writer.Write(WorldLocation2Id, 17u);
    }
}
