using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetMoveDefaults)]
[WireStruct]
[UnionCase(EntityCommand.SetMoveDefaults)]
public sealed partial class SetMoveDefaultsCommand : IEntityCommandModel
{
    [UInt(1)] public bool Blend { get; set; }
}
