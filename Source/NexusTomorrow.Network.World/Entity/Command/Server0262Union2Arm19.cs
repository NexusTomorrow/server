using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.State;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(25)]
public sealed partial class Server0262Union2Arm19 : IServer0262Union2
{
    [UInt(8), Count] public byte Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown2 { get; set; } = [];
    [UInt(32)] public uint Unknown3 { get; set; }
}
