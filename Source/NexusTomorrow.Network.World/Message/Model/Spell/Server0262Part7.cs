using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
public sealed partial class Server0262Part7
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(18)] public uint Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
    [Array(CountBits = 32)] public List<Server0262Part8> Unknown5 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part12> Unknown6 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part13> Unknown7 { get; set; } = [];
}
