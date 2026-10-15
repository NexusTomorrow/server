using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireStruct]
public sealed partial class HazardModifier : IWritable
{
    [Float32] public float Multiplier { get; set; }
    [Float32] public float Offset { get; set; }
    [UInt(1)] public bool Suspended { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Multiplier);
        writer.Write(Offset);
        writer.Write(Suspended);
    }
}
