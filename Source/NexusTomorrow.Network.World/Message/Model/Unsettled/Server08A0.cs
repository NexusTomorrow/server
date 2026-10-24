using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server08A0, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server08A0
{
    [WireStruct]
    public sealed partial class Server08A0Part0
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(64)] public ulong Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(32)] public uint Unknown11 { get; set; }
    [Array(CountBits = 32)] public List<Server08A0Part0> Unknown12 { get; } = [];
}
