using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemLocation : IReadable, IWritable
{
    [UInt(9)] public InventoryLocation Location { get; set; }
    [UInt(32)] public uint BagIndex { get; set; }

    public void Read(GamePacketReader reader)
    {
        Location = reader.ReadEnum<InventoryLocation>(9u);
        BagIndex = reader.ReadUInt();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Location, 9u);
        writer.Write(BagIndex);
    }
}
