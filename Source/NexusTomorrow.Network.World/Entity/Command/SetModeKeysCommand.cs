using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetModeKeys)]
[WireStruct]
[UnionCase(EntityCommand.SetModeKeys)]
public sealed partial class SetModeKeysCommand : IEntityCommandModel
{
    [UInt(8), Count] public byte TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<ModeType> Modes { get; set; } = [];
    [UInt(32)] public uint Offset { get; set; }
}
