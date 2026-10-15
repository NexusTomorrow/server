using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.ServerICCommDirectedMessage, Direction.ServerToClient)]
public sealed partial class ServerICCommDirectedMessage
{
    [UInt(64)] public ulong IccomId { get; set; }
    [NarrowString] public string Message { get; set; }
    [WideString] public string SenderName { get; set; }
}

