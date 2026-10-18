using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Message081A, Direction.Both)]
public sealed partial class Message081A
{
    [WireStruct]
    public sealed partial class Message081APart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(32)] public uint Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(15)] public ushort Unknown1 { get; set; }
    [UInt(2)] public byte Unknown2 { get; set; }
    [Array(CountBits = 32)] public List<Message081APart0> Unknown3 { get; } = [];
}
