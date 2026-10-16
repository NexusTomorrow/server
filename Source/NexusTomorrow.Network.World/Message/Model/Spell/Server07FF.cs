using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07FF, Direction.ServerToClient)]
public sealed partial class Server07FF
{
    [WireStruct]
    public sealed partial class Server07FFPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(8)] public byte Unknown1 { get; set; }
        public Client0096Part0 Unknown2 { get; set; } = new Client0096Part0();
        [Float32] public float Unknown3 { get; set; }
        [Float32] public float Unknown4 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server07FFPart1
    {
        [UInt(16)] public ushort Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(8)] public byte Unknown2 { get; set; }
        public Client0096Part0 Unknown3 { get; set; } = new Client0096Part0();
        [Float32] public float Unknown4 { get; set; }
        [Float32] public float Unknown5 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    [UInt(18)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(16)] public ushort Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    public Client0096Part0 Unknown7 { get; set; } = new Client0096Part0();
    [Float32] public float Unknown8 { get; set; }

    [Array(CountBits = 8)] public List<Server07FFPart0> Unknown9 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server07FFPart1> Unknown10 { get; set; } = [];
    [UInt(1)] public bool Unknown11 { get; set; }
    [UInt(1)] public bool Unknown12 { get; set; }
}
