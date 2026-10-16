using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's characters and its character creation options, the answer to <see cref="CharacterListRequest"/>. The client
/// replaces its list with this one.
/// </summary>
[WireMessage(GameMessageOpcode.Server0117, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterList
{
    [WireStruct]
    public sealed partial class Server0117Part0
    {
        [WireStruct]
        public sealed partial class Server0117Part2
        {
            public Client0096Part0 Unknown0 { get; set; } = new Client0096Part0();
            [Float32] public float Unknown1 { get; set; }
            [Float32] public float Unknown2 { get; set; }
        }

        [UInt(64)] public ulong Unknown0 { get; set; }
        [WideString] public string Unknown1 { get; set; } = "";
        [UInt(2)] public byte Unknown2 { get; set; }
        [UInt(5)] public byte Unknown3 { get; set; }
        [UInt(5)] public byte Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [Array(CountBits = 32)] public List<Server0117Part1> Unknown7 { get; } = [];
        [Array(CountBits = 32)] public List<Server0117Part1> Unknown8 { get; } = [];
        [UInt(15)] public ushort Unknown9 { get; set; }
        [UInt(15)] public ushort Unknown10 { get; set; }
        [UInt(14)] public ushort Unknown11 { get; set; }
        public Server0117Part2 Unknown12 { get; } = new();
        [UInt(3)] public byte Unknown13 { get; set; }
        [UInt(1)] public bool Unknown14 { get; set; }
        [UInt(1)] public bool Unknown15 { get; set; }
        [UInt(32)] public uint Unknown16 { get; set; }
        [UInt(4), Count] public byte Unknown17 { get; set; }
        [Array(CountFrom = nameof(Unknown17)), UInt(32)] public List<uint> Unknown18 { get; } = [];
        [Array(CountFrom = nameof(Unknown17)), UInt(32)] public List<uint> Unknown19 { get; } = [];
        [Array(CountBits = 32), Float32] public List<float> Unknown20 { get; } = [];
        [Float32] public float Unknown21 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0117Part0> Unknown1 { get; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown2 { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown3 { get; set; } = [];
    [UInt(14)] public ushort Unknown4 { get; set; }
    public Client00B5Part0 Unknown5 { get; set; } = new Client00B5Part0();
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }
    [UInt(14)] public ushort Unknown10 { get; set; }
    [UInt(1)] public bool Unknown11 { get; set; }
}
