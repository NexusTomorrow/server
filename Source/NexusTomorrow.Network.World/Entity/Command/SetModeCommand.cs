using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetMode)]
[WireStruct]
[UnionCase(EntityCommand.SetMode)]
public sealed partial class SetModeCommand : IEntityCommandModel
{
    [UInt(32)] public ModeType Mode { get; set; }
}
