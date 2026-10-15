using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerP2PTradeInvite)]
[WireMessage(GameMessageOpcode.ServerP2PTradeInvite, Direction.ServerToClient)]
public sealed partial class ServerP2PTradeInvite : IWritable
{
    [UInt(32)] public uint TradeInviterUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TradeInviterUnitId);
    }
}
