using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellVisualGroupAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualGroupAdd
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(16)] public ushort Spell4VisualGroupId { get; set; }
    [UInt(32)] public uint Unknown { get; set; } // mostly 0, sometimes 1 TODO: research more
    [Array(CountBits = 32), UInt(32)] public List<uint> SpellVisualEffectUniqueIds { get; set; } = [];
}
