using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetModeDefault)]
[WireStruct]
[UnionCase(EntityCommand.SetModeDefault)]
public sealed partial class SetModeDefaultCommand : IEntityCommandModel
{
    [UInt(1)] public bool Unused { get; set; }
}
