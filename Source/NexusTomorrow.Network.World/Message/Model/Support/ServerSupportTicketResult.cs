using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.ServerSupportTicketResult, Direction.ServerToClient)]
public sealed partial class ServerSupportTicketResult
{
    [UInt(1)] public bool Success { get; set; }
}
