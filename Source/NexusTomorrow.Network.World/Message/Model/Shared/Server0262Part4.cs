using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part4
{
    public interface IServer0262Union1 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server0262Union1Arm00 : IServer0262Union1
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server0262Union1Arm01 : IServer0262Union1
    {
        [Float32] public float Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server0262Union1Arm02 : IServer0262Union1
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [Union(nameof(Unknown1))] public IServer0262Union1 Unknown2 { get; set; } = new Server0262Union1Arm00();
}
