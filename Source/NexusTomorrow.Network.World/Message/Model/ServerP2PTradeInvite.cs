using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerP2PTradeInvite)]
    public class ServerP2PTradeInvite : IWritable
    {
        public uint TradeInviterUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TradeInviterUnitId);
        }
    }
}
