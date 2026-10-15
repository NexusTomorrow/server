using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationSpin)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationSpin)]
public sealed partial class SetRotationSpinCommand : IEntityCommandModel
{
    [Float32] public Vector3 Rotation { get; set; }
    [UInt(32)] public uint FlightTime { get; set; }
    [PackedFloat] public float Spin { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
