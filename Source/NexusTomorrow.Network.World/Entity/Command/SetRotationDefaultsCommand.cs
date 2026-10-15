using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationDefaults)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationDefaults)]
public sealed partial class SetRotationDefaultsCommand : IEntityCommandModel
{
    [UInt(1)] public bool Blend { get; set; }
}
