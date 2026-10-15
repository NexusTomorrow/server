using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.ServerICCommChannelLeft, Direction.ServerToClient)]
public sealed partial class ServerICCommChannelLeft
{
    [UInt(64)] public ulong IccomId { get; set; }
}
