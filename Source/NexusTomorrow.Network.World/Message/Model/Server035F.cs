using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server035F, Direction.ServerToClient)]
public sealed partial class Server035F
{
    [WireStruct]
    public sealed partial class Server035FPart0
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(1)] public bool Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server035FPart1
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server035FPart2
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32), Count] public uint Unknown4 { get; set; }
        [Array(CountFrom = nameof(Unknown4)), UInt(32)] public List<uint> Unknown5 { get; set; } = [];
        [UInt(32)] public uint Unknown6 { get; set; }
        [Array(CountFrom = nameof(Unknown4)), UInt(32)] public List<uint> Unknown7 { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<Server035FPart0> Unknown0 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server035FPart1> Unknown1 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server035FPart2> Unknown2 { get; set; } = [];

    [UInt(64)] public ulong Unknown3 { get; set; }
}
