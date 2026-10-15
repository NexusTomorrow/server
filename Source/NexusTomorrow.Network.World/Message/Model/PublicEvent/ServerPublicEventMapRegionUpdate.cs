using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Unlike PublicEVentLocationUpdate, only allows for adding/removing to PublicEventObjectives, not to the PublicEvent
[Message(GameMessageOpcode.ServerPublicEventMapRegionUpdate)]
[WireMessage(GameMessageOpcode.ServerPublicEventMapRegionUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventMapRegionUpdate : IWritable
{
    [UInt(32)] public uint ObjectiveId { get; set; }
    [UInt(3)] public PublicEventOperationType Operation { get; set; }
    public MapRegion MapRegion { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectiveId, 32u);
        writer.Write(Operation, 3u);
        MapRegion.Write(writer);
    }
}
