using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationSpline)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationSpline)]
public sealed partial class SetRotationSplineCommand : IEntityCommandModel
{
    [UInt(32)] public uint SplineId { get; set; }
    [UInt(16)] public ushort Speed { get; set; }
    [Float32] public float Position { get; set; }
    [UInt(4)] public byte Mode { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
    [UInt(1)] public bool AdjustSpeedToLength { get; set; }
}
