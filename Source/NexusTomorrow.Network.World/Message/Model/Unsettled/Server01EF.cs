using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server01EF, Direction.ServerToClient)]
public sealed partial class Server01EF
{
    [WireStruct]
    public sealed partial class Server01EFPart0
    {
        [WireStruct]
        public sealed partial class Server01EFPart1
        {
            [UInt(5)] public byte Unknown0 { get; set; }
            [UInt(32)] public uint Unknown1 { get; set; }
            [UInt(32)] public uint Unknown2 { get; set; }
        }

        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(14)] public ushort Unknown1 { get; set; }
        [UInt(64)] public ulong Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(8)] public byte Unknown5 { get; set; }
        [Array(CountBits = 8)] public List<Server01EFPart1> Unknown6 { get; } = [];
    }

    [Array(CountBits = 8)] public List<Server01EFPart0> Unknown0 { get; } = [];
}
