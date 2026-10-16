using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server07F2, Direction.ServerToClient)]
public sealed partial class Server07F2
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [Array(CountBits = 32)] public List<Server07F2Part0> Unknown3 { get; set; } = [];
}
