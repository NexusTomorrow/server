using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class TriggerEntityModel : IEntityModel
{
    [WireStruct]
    public sealed partial class Bound : IWritable
    {
        [UInt(2)] public byte Type { get; set; }
        public Position Min { get; set; }
        public Position Lim { get; set; }
        [Float32] public float Radius { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Type, 2);
            Min.Write(writer);
            Lim.Write(writer);
            writer.Write(Radius);
        }
    }

    [WideString] public string Name { get; set; }
    [Array(CountBits = 8)] public List<Bound> Bounds { get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Name);
        writer.Write((byte)Bounds.Count, 8);
        Bounds.ForEach(s => s.Write(writer));
    }
}
