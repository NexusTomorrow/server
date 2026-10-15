using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionKeys)]
[WireStruct]
[UnionCase(EntityCommand.SetPositionKeys)]
public sealed partial class SetPositionKeysCommand : IEntityCommandModel
{
    [UInt(10), Count] public ushort TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), Float32] public List<Vector3> Positions { get; set; } = [];
    [UInt(2)] public byte Type { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
