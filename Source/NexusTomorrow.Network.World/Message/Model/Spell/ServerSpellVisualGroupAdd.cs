using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0264, Direction.ServerToClient)]
public sealed partial class Server0264
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown3 { get; set; } = [];
}
