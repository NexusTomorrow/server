using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetScaleKeys)]
[WireStruct]
[UnionCase(EntityCommand.SetScaleKeys)]
public sealed partial class SetScaleKeysCommand : IEntityCommandModel
{
    [UInt(8), Count] public byte TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), PackedFloat] public List<float> Scales { get; set; } = [];
    [UInt(2)] public byte Type { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
