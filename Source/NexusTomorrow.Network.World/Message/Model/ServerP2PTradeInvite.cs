using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerP2PTradeInvite, Direction.ServerToClient)]
public sealed partial class ServerP2PTradeInvite
{
    [UInt(32)] public uint TradeInviterUnitId { get; set; }
}
