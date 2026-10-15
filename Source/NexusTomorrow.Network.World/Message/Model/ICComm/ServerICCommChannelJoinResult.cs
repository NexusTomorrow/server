using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

// Presumably sent if a client is not allowed to join a channel but none seen in sniffs
[WireMessage(GameMessageOpcode.ServerICCommChannelJoinResult, Direction.ServerToClient)]
public sealed partial class ServerICCommChannelJoinResult
{
    [UInt(3)] public ICCommChannelType Type { get; set; }
    [UInt(4)] public ICCommJoinResult Result { get; set; }
    [WideString] public string ChannelName { get; set; }
}
