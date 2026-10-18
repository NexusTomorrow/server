using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server016B, Direction.ServerToClient)]
public sealed partial class Server016B
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(4), Count] public byte Unknown2 { get; set; }
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown3 { get; } = [];
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown4 { get; } = [];
}
