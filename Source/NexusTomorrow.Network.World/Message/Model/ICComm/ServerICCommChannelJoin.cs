using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.ServerICCommChannelJoin, Direction.ServerToClient)]
public sealed partial class ServerICCommChannelJoin
{
    [UInt(3)] public ICCommChannelType Type { get; set; }
    [UInt(64)] public ulong IccomId { get; set; }
    [WideString] public string ChannelName { get; set; }
}
