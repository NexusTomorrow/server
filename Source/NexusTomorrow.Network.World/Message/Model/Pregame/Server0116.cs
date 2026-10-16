using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server0116, Direction.ServerToClient)]
public sealed partial class Server0116
{
    [WireStruct]
    public sealed partial class Server0116Part0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [WideString] public string Unknown2 { get; set; } = "";
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(14)] public ushort Unknown4 { get; set; }
        [UInt(14)] public ushort Unknown5 { get; set; }
        [UInt(2)] public ushort Unknown6 { get; set; }
        [UInt(14)] public ushort Unknown7 { get; set; }
        [UInt(3)] public ushort Unknown8 { get; set; }
        [UInt(15)] public ushort Unknown9 { get; set; }
        [UInt(15)] public ushort Unknown10 { get; set; }
        [Float32] public float Unknown11 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0116Part0> Unknown0 { get; set; } = [];
}
