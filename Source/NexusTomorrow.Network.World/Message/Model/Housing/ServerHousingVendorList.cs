using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingVendorList)]
[WireMessage(GameMessageOpcode.ServerHousingVendorList, Direction.ServerToClient)]
public sealed partial class ServerHousingVendorList : IWritable
{
    [WireStruct]
    public sealed partial class PlugItem : IWritable
    {
        [UInt(64)] public ulong SourceId { get; set; }
        [UInt(32)] public uint PlugItemId { get; set; }
        [UInt(32)] public uint Cost { get; set; }
        [UInt(32)] public uint PlugItemFlags { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(SourceId);
            writer.Write(PlugItemId);
            writer.Write(Cost);
            writer.Write(PlugItemFlags);
        }
    }

    [Array(CountBits = 32)] public List<PlugItem> PlugItems { get; set; } = [];
    [UInt(2)] public byte ListType { get; set; } = 0; // must be zero or else the client ignores the contents of the message

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PlugItems.Count);
        PlugItems.ForEach(p => p.Write(writer));
        writer.Write(ListType, 2);
    }
}
