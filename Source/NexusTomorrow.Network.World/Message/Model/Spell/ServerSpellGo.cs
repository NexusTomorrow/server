using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07F4, Direction.ServerToClient)]
public sealed partial class Server07F4
{
    [WireStruct]
    public sealed partial class Server07F4Part0
    {
        public Client0096Part0 Unknown0 { get; set; } = new Client0096Part0();
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        public Client0096Part0 Unknown3 { get; set; } = new Client0096Part0();
        [UInt(1)] public bool Unknown4 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    public Client0096Part0 Unknown2 { get; set; } = new Client0096Part0();

    [Array(CountBits = 8)] public List<Server0262Part8> Unknown3 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part12> Unknown4 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part13> Unknown5 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server07F4Part0> Unknown6 { get; set; } = [];

    [SInt(8)] public sbyte Unknown7 { get; set; }
}
