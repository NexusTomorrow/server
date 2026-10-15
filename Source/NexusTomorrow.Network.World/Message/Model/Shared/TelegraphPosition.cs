using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class TelegraphPosition
{
    [UInt(16)] public ushort TelegraphId { get; set; }
    [UInt(32)] public uint AttachedUnitId { get; set; }
    [UInt(8)] public byte TargetFlags { get; set; }
    public Position Position { get; set; } = new Position();
    [Float32] public float Yaw { get; set; }
    [Float32] public float Pitch { get; set; }
}
