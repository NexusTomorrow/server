using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Only used to trigger a UI event. Not seen in sniffs but perfectly usable
[Message(GameMessageOpcode.ServerPublicEventObjectiveStart)]
[WireMessage(GameMessageOpcode.ServerPublicEventObjectiveStart, Direction.ServerToClient)]
public sealed partial class ServerPublicEventObjectiveStart : IWritable
{
    [UInt(15)] public uint ObjectiveId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectiveId, 15);
    }
}
