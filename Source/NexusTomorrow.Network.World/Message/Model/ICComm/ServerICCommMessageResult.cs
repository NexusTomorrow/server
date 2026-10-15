using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.ServerICCommMessageResult, Direction.ServerToClient)]
public sealed partial class ServerICCommMessageResult
{
    [UInt(64)] public ulong IccomId { get; set; }
    [UInt(32)] public uint MessageId { get; set; }
    [UInt(4)] public ICCommMessageResult Result { get; set; }
}
