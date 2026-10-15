using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerP2PTradeItemRemoved, Direction.ServerToClient)]
public sealed partial class ServerPTPTradeItemRemoved
{
    [UInt(64)] public ulong ItemGuid { get; set; }
}
