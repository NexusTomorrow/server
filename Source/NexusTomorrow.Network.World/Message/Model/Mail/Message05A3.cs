using NexusTomorrow.Game.Static.Mail;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Message05A3, Direction.ServerToClient)]
public sealed partial class Message05A3
{
    [WireStruct]
    public sealed partial class Message05A3Part1
    {
        [UInt(18)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(64)] public ulong Unknown5 { get; set; }
        [UInt(18)] public uint Unknown6 { get; set; }
        [FixedArray(5), UInt(32)] public uint[] Unknown7 { get; set; } = new uint[5];
        [FixedArray(8), UInt(32)] public uint[] Unknown8 { get; set; } = new uint[8];
    }

    [WireStruct]
    public sealed partial class Message05A3Part0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(14)] public ushort Unknown2 { get; set; }
        [WideString] public string Unknown3 { get; set; }
        [WideString] public string Unknown4 { get; set; }
        [UInt(21)] public uint Unknown5 { get; set; }
        [UInt(21)] public uint Unknown6 { get; set; }
        [UInt(18)] public uint Unknown7 { get; set; }
        [UInt(4)] public byte Unknown8 { get; set; }
        [UInt(64)] public ulong Unknown9 { get; set; }
        [UInt(64)] public ulong Unknown10 { get; set; }
        [Float32] public float Unknown11 { get; set; }
        [UInt(32)] public uint Unknown12 { get; set; }
        public Client00B5Part0 Unknown13 { get; set; } = new();
        [Array(CountBits = 32)] public List<Message05A3Part1> Unknown14 { get; set; } = [];
    }

    [UInt(1)] public bool Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Message05A3Part0> Unknown1 { get; set; } = [];
}
