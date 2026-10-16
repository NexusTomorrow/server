using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0359, Direction.ServerToClient)]
public sealed partial class Server0359
{
    public interface IServer0359Union0 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server0359Union0Arm00 : IServer0359Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server0359Union0Arm01 : IServer0359Union0
    {
        [WideString] public string Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(3)]
    public sealed partial class Server0359Union0Arm03 : IServer0359Union0
    {
        [UInt(1)] public bool Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(4)]
    public sealed partial class Server0359Union0Arm04 : IServer0359Union0
    {
        [UInt(18)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(5)]
    public sealed partial class Server0359Union0Arm05 : IServer0359Union0
    {
        [UInt(15)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server0359Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0359Union0 Unknown1 { get; set; }
    }

    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0359Part0> Unknown1 { get; set; } = [];
}
