using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellVisualGroupAdd)]
[WireMessage(GameMessageOpcode.ServerSpellVisualGroupAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualGroupAdd : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(16)] public ushort Spell4VisualGroupId { get; set; }
    [UInt(32)] public uint Unknown { get; set; } // mostly 0, sometimes 1 TODO: research more
    [Array(CountBits = 32), UInt(32)] public List<uint> SpellVisualEffectUniqueIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Spell4VisualGroupId);
        writer.Write(Unknown);
        writer.Write(SpellVisualEffectUniqueIds.Count);
        SpellVisualEffectUniqueIds.ForEach(id => writer.Write(id));
    }
}
