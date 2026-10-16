using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server0853, NexusTomorrow.Network.Codec.Direction.ServerToClient)]
public sealed partial class Server0853
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(15)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(4)] public byte Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown7 { get; set; } = [];
}
