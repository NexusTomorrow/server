using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.Message00C7, Direction.ServerToClient)]
public sealed partial class Message00C7
{
    [WireStruct]
    public sealed partial class Message00C7Part0
    {
        [UInt(14)] public uint Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(32)] public uint Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }
        [UInt(32)] public uint Unknown10 { get; set; }
        [UInt(1)] public bool Unknown11 { get; set; }
        [UInt(1)] public bool Unknown12 { get; set; }
        [UInt(1)] public bool Unknown13 { get; set; }
        [UInt(1)] public bool Unknown14 { get; set; }
        [UInt(32)] public uint Unknown15 { get; set; }
        [UInt(32)] public uint Unknown16 { get; set; }
        [UInt(32)] public uint Unknown17 { get; set; }
        [UInt(32)] public uint Unknown18 { get; set; }
        [UInt(32)] public uint Unknown19 { get; set; }
        [UInt(32)] public uint Unknown20 { get; set; }
        [FixedArray(3), UInt(32)] public uint[] Unknown21 { get; set; } = new uint[3];
    }

    [Array(CountBits = 32)] public List<Message00C7Part0> Unknown0 { get; set; } = [];
}
