using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

[WireMessage(GameMessageOpcode.Server098E, Direction.ServerToClient)]
public sealed partial class Server098E
{
    [WireStruct]
    public sealed partial class Server098EPart0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
        [UInt(5)] public byte Unknown2 { get; set; }
        [UInt(3)] public byte Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [WideString] public string Unknown5 { get; set; } = "";
        [UInt(1)] public bool Unknown6 { get; set; }
        [UInt(64)] public ulong Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server098EPart0> Unknown0 { get; } = [];
}
