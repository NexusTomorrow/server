using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerP2PTradeUpdateItem)]
[WireMessage(GameMessageOpcode.ServerP2PTradeUpdateItem, Direction.ServerToClient)]
public sealed partial class ServerP2PTradeUpdateItem : IWritable
{
    [UInt(32)] public uint TradeIndex { get; set; }
    [UInt(32)] public uint OwnerUnitId { get; set; }
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(64)] public ulong ItemGuid { get; set; }
    [UInt(32)] public uint Quantity { get; set; }
    [UInt(64)] public ulong Unknown1_64bit { get; set; }
    [UInt(32)] public uint Unknown2_32bit { get; set; }
    [UInt(64)] public ulong Unknown3_64bit { get; set; }
    [UInt(18)] public uint Unknown4_18bit { get; set; }
    [FixedArray(5), UInt(32)] public uint[] UnknownArray { get; set; } = new uint[5];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TradeIndex);
        writer.Write(OwnerUnitId);
        writer.Write(Item2Id, 18);
        writer.Write(ItemGuid);
        writer.Write(Quantity);
        writer.Write(Unknown1_64bit);
        writer.Write(Unknown2_32bit);
        writer.Write(Unknown3_64bit);
        writer.Write(Unknown4_18bit, 18);
        foreach (uint unknown in UnknownArray)
        {
            writer.Write(unknown);
        }
    }
}
