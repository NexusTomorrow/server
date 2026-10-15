using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerP2PTradeUpdateMoney, Direction.ServerToClient)]
public sealed partial class ServerP2PTradeUpdateMoney
{
    [UInt(64)] public ulong Credits { get; set; }
    [UInt(32)] public uint UnitId { get; set; } // UnitId of whichever trade partner updated their money offer
}
