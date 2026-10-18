using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server034E, Direction.ServerToClient)]
public sealed partial class Server034E
{
    [WireStruct]
    public sealed partial class Server034EPart0
    {
        [WideString] public string Unknown0 { get; set; } = "";
        [UInt(64)] public ulong Unknown1 { get; set; }
        [UInt(64)] public ulong Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
        [UInt(64)] public ulong Unknown4 { get; set; }
        [UInt(64)] public ulong Unknown5 { get; set; }
    }

    [WideString] public string Unknown0 { get; set; } = "";
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [Array(CountBits = 32)] public List<Server034EPart0> Unknown5 { get; } = [];
}
