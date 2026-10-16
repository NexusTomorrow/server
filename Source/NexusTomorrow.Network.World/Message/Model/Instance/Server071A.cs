using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server071A, Direction.ServerToClient)]
public sealed partial class Server071A
{
    [WireStruct]
    public sealed partial class Server071APart0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(15)] public ushort Unknown1 { get; set; }
        [UInt(64)] public ulong Unknown2 { get; set; }
        [Float32] public float Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server071APart0> Unknown0 { get; set; } = [];
}
