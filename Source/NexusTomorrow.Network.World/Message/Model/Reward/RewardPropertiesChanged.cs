using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server092C, Direction.ServerToClient)]
public sealed partial class RewardPropertiesChanged
{
    public interface IServer092CUnion0 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server092CUnion0Arm00 : IServer092CUnion0
    {
        [Float32] public float Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server092CUnion0Arm01 : IServer092CUnion0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server092CUnion0Arm02 : IServer092CUnion0
    {
        [Float32] public float Unknown0 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server092CPart0
    {
        [WireStruct]
        public sealed partial class Server092CPart1
        {
            [UInt(4)] public byte Unknown0 { get; set; }
            [UInt(32)] public uint Unknown1 { get; set; }
            [UInt(2)] public byte Unknown2 { get; set; }
            [Union(nameof(Unknown2))] public IServer092CUnion0 Unknown3 { get; set; } = new Server092CUnion0Arm00();
        }

        [UInt(6)] public byte Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(2)] public byte Unknown2 { get; set; }
        [Union(nameof(Unknown2))] public IServer092CUnion0 Unknown3 { get; set; } = new Server092CUnion0Arm00();
        [Array(CountBits = 8)] public List<Server092CPart1> Unknown4 { get; set; } = [];
    }

    [Array(CountBits = 8)] public List<Server092CPart0> Unknown0 { get; set; } = [];
}
