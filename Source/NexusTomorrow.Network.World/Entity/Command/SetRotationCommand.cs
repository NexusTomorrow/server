using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotation)]
[WireStruct]
[UnionCase(EntityCommand.SetRotation)]
public sealed partial class SetRotationCommand : IEntityCommandModel
{
    [Float32] public Vector3 Rotation { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
