using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Identity : IReadable, IWritable
{
    [UInt(14)] public ushort RealmId { get; set; }
    [UInt(64)] public ulong Id { get; set; }

    public void Read(GamePacketReader reader)
    {
        RealmId = reader.ReadUShort(14u);
        Id = reader.ReadULong();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RealmId, 14u);
        writer.Write(Id);
    }

    public override string ToString()
    {
        return $"{RealmId}:{Id}";
    }
}
