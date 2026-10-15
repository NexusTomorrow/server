using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationFaceUnit)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationFaceUnit)]
public sealed partial class SetRotationFaceUnitCommand : IEntityCommandModel
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
