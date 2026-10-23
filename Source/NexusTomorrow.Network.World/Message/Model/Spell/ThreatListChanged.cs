using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0909, Direction.ServerToClient)]
public sealed partial class ThreatListChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [FixedArray(5), UInt(32)] public uint[] Unknown1 { get; set; } = new uint[5];
    [FixedArray(5), UInt(32)] public uint[] Unknown2 { get; set; } = new uint[5];
}
