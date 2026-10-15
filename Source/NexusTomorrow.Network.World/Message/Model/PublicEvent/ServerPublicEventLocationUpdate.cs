using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventLocationUpdate)]
[WireMessage(GameMessageOpcode.ServerPublicEventLocationUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventLocationUpdate : IWritable
{
    [UInt(14)] public uint ObjectId { get; set; } // Can be either PublicEventId or PublicEventObjectiveId depending on the operation
    [UInt(3)] public PublicEventOperationType Operation { get; set; }
    [UInt(17)] public uint WorldLocation2Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectId, 14u);
        writer.Write(Operation, 3u);
        writer.Write(WorldLocation2Id, 17u);
    }
}
