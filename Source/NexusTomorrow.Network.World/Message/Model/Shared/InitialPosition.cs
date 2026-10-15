using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class InitialPosition
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(8)] public byte TargetFlags { get; set; }
    public Position Position { get; set; } = new Position();
    [Float32] public float Yaw { get; set; }
    [Float32] public float Pitch { get; set; }
}
