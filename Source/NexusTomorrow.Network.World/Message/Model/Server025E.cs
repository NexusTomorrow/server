using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using FactionId = NexusTomorrow.Game.Static.Reputation.Faction;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server025E, Direction.ServerToClient)]
public sealed partial class Server025E
{
    [WireStruct]
    public sealed partial class Server025EPart0
    {
        [WireStruct]
        public sealed partial class Server025EPart1
        {
            [UInt(14)] public ushort Unknown0 { get; set; }
            [Float32] public float Unknown1 { get; set; }
        }

        [UInt(14)] public ushort Unknown0 { get; set; }
        [Array(CountBits = 16)] public List<Server025EPart1> Unknown1 { get; set; } = [];
    }

    [WireStruct]
    public sealed partial class Server025EPart2
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(18)] public uint Unknown1 { get; set; }
        [UInt(5)] public byte Unknown2 { get; set; }
        [UInt(5)] public byte Unknown3 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server025EPart3
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0111Part0> Unknown0 { get; } = [];
    [FixedArray(16), UInt(64)] public ulong[] Unknown1 { get; } = new ulong[16];
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(3)] public byte Unknown7 { get; set; }
    [UInt(16)] public ushort Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }
    public Server025EPart0 Unknown10 { get; set; }
    [Array(CountBits = 32)] public List<Server025EPart2> Unknown11 { get; } = [];
    [UInt(32)] public uint Unknown12 { get; set; }
    [UInt(16)] public ushort Unknown13 { get; set; }
    [UInt(32)] public uint Unknown14 { get; set; }
    [UInt(32)] public uint Unknown15 { get; set; }
    [UInt(32)] public uint Unknown16 { get; set; }
    [Array(CountBits = 6), UInt(16)] public List<ushort> Unknown17 { get; } = [];
    [FixedArray(512), UInt(16)] public ushort[] Unknown18 { get; set; } = new ushort[512];
    [Float32] public float Unknown19 { get; set; }
    [UInt(1)] public bool Unknown20 { get; set; }
    [UInt(32)] public uint Unknown21 { get; set; }
    [Array(CountBits = 32)] public List<Server025EPart3> Unknown22 { set; get; } = [];
}
