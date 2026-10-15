using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventObjectiveStatusUpdate)]
[WireMessage(GameMessageOpcode.ServerPublicEventObjectiveStatusUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventObjectiveStatusUpdate : IWritable
{
    [UInt(15)] public uint ObjectiveId { get; set; }
    public PublicEventObjectiveStatus ObjectiveStatus { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectiveId, 15);
        ObjectiveStatus.Write(writer);
    }
}
