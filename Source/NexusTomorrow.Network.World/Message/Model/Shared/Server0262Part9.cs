using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part9
{
    public interface IServer0262Union3 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server0262Union3Arm00 : IServer0262Union3
    {
        [UInt(1)] public byte Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server0262Union3Arm01 : IServer0262Union3
    {
        public Server0262Part10 Unknown0 { get; set; } = new();
    }

    [UInt(19)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(2)] public byte Unknown4 { get; set; }
    [Union(nameof(Unknown4))] public IServer0262Union3 Unknown5 { get; set; } = new Server0262Union3Arm00();
}
