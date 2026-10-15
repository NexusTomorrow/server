using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetMoveKeys)]
[WireStruct]
[UnionCase(EntityCommand.SetMoveKeys)]
public sealed partial class SetMoveKeysCommand : IEntityCommandModel
{
    [UInt(10), Count] public ushort TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), PackedFloat] public List<Vector3> Moves { get; set; } = [];
    [UInt(2)] public byte Type { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
}
