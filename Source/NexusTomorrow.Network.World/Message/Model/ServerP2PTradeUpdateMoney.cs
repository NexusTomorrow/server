using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerP2PTradeUpdateMoney)]
public class ServerP2PTradeUpdateMoney : IWritable
{
    public ulong Credits { get; set; }
    public uint UnitId { get; set; } // UnitId of whichever trade partner updated their money offer

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Credits);
        writer.Write(UnitId);
    }
}
