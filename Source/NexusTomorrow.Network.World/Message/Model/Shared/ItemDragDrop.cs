using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemDragDrop : IReadable, IWritable
{
    [UInt(64)] public ulong Guid { get; set; }
    [UInt(64)] public ulong DragDrop { get; set; }

    public void Read(GamePacketReader reader)
    {
        Guid = reader.ReadULong();
        DragDrop = reader.ReadULong();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(DragDrop);
    }
}
