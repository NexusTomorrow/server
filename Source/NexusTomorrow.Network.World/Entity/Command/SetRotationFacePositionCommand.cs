using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationFacePosition)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationFacePosition)]
public sealed partial class SetRotationFacePositionCommand : IEntityCommandModel
{
    [Float32] public Vector3 Position { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
