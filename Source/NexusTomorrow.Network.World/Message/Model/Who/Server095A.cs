using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Who;

[WireMessage(GameMessageOpcode.Server095A, Direction.ServerToClient)]
public sealed partial class Server095A
{
    [WireStruct]
    public sealed partial class Server095APart0
    {
        [WideString] public string Unknown0 { get; set; } = "";
        [WideString] public string Unknown1 { get; set; } = "";
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(5)] public byte Unknown3 { get; set; }
        [UInt(5)] public byte Unknown4 { get; set; }
        [UInt(2)] public byte Unknown5 { get; set; }
        [UInt(14)] public ushort Unknown6 { get; set; }
        [UInt(3)] public byte Unknown7 { get; set; }
        [UInt(15)] public uint Unknown8 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server095APart0> Unknown0 { get; set; } = [];
    [UInt(2)] public byte Unknown1 { get; set; }
}
