using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0112Part1
{
    [WireStruct]
    public sealed partial class Server0112Part2
    {
        [UInt(14)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    public interface IServer0112Union0 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server0112Union0Arm00 : IServer0112Union0
    {
        [UInt(8)] public byte Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server0112Union0Arm01 : IServer0112Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server0112Union0Arm02 : IServer0112Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(3)]
    public sealed partial class Server0112Union0Arm03 : IServer0112Union0
    {
        [Array(CountBits = 32)] public List<Server0112Part2> Unknown0 { get; set; } = [];
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
    [Union(nameof(Unknown5))] public IServer0112Union0 Unknown6 { get; set; } = new Server0112Union0Arm00();
}
