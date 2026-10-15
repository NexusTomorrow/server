using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Spline;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionPath)]
[WireStruct]
[UnionCase(EntityCommand.SetPositionPath)]
public sealed partial class SetPositionPathCommand : IEntityCommandModel
{
    [Array(CountBits = 10), Float32] public List<Vector3> Positions { get; set; } = [];
    [PackedFloat] public float Speed { get; set; }
    [UInt(2)] public SplineType Type { get; set; }
    [UInt(4)] public SplineMode Mode { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
