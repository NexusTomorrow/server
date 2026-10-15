using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.State;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetStateKeys)]
[WireStruct]
[UnionCase(EntityCommand.SetStateKeys)]
public sealed partial class SetStateKeysCommand : IEntityCommandModel
{
    [UInt(8), Count] public byte TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<StateFlags> States { get; set; } = [];
    [UInt(32)] public uint Offset { get; set; }
}
