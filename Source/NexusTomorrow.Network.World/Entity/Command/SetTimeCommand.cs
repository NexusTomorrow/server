using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetTime)]
[WireStruct]
[UnionCase(EntityCommand.SetTime)]
public sealed partial class SetTimeCommand : IEntityCommandModel
{
    [UInt(32)] public uint Time { get; set; }
}
