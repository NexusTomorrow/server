using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventTimeUpdate)]
[WireMessage(GameMessageOpcode.ServerPublicEventTimeUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventTimeUpdate : IWritable
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(1)] public bool Busy { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14u);
        writer.Write(Busy);
        writer.Write(ElapsedTimeMs);
    }
}
