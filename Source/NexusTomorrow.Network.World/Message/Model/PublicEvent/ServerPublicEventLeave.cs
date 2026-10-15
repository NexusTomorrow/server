using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventLeave)]
[WireMessage(GameMessageOpcode.ServerPublicEventLeave, Direction.ServerToClient)]
public sealed partial class ServerPublicEventLeave : IWritable
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [UInt(32)] public PublicEventRemoveReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PublicEventId, 14);
        writer.Write(Reason, 32);
    }
}
