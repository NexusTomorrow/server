using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server00D6, Direction.ServerToClient)]
public sealed partial class Server00D6
{
    [WireStruct]
    public sealed partial class Server00D6Part3
    {
        [UInt(15)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    public Server00D6Part0 Unknown3 { get; set; } = new();
    [Array(CountBits = 32)] public List<Server00D6Part1> Unknown4 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server00D6Part2> Unknown5 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server00D6Part3> Unknown6 { get; set; } = [];
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [FixedArray(3), UInt(32)] public uint[] Unknown9 { get; set; } = new uint[3];
}
