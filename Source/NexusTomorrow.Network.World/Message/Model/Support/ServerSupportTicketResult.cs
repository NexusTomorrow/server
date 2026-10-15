using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[Message(GameMessageOpcode.ServerSupportTicketResult)]
[WireMessage(GameMessageOpcode.ServerSupportTicketResult, Direction.ServerToClient)]
public sealed partial class ServerSupportTicketResult : IWritable
{
    [UInt(1)] public bool Success { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Success);
    }
}
