using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Spline;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionMultiSpline)]
[WireStruct]
[UnionCase(EntityCommand.SetPositionMultiSpline)]
public sealed partial class SetPositionMultiSplineCommand : IEntityCommandModel
{
    [Array(CountBits = 10), UInt(32)] public List<uint> SplineIds { get; set; } = [];
    [PackedFloat] public float Speed { get; set; }
    [Float32] public float Position { get; set; }
    [Float32] public float TakeoffLocationHeight { get; set; }
    [Float32] public float LandingLocationHeight { get; set; }
    [PackedFloat] public Vector3 FormationData { get; set; }
    [UInt(4)] public SplineMode Mode { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(32)] public uint MultiSplineFlags { get; set; }
    [UInt(1)] public bool Blend { get; set; }
    [UInt(1)] public bool IsContinuing { get; set; }
}
