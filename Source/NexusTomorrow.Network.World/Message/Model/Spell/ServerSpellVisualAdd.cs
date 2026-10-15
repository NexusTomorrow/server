using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellVisualAdd)]
[WireMessage(GameMessageOpcode.ServerSpellVisualAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualAdd : IWritable
{
    [UInt(32)] public uint SpellVisualEffectClientId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(11)] public uint VisualEffectId { get; set; }
    [UInt(11)] public uint VisualEffectIdSound { get; set; }
    [UInt(11)] public uint Spell4VisualId { get; set; }
    [UInt(32)] public uint Unk14 { get; set; }
    public Position Position { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellVisualEffectClientId);
        writer.Write(UnitId);
        writer.Write(VisualEffectId, 11);
        writer.Write(VisualEffectIdSound, 11);
        writer.Write(Spell4VisualId, 11);
        writer.Write(Unk14);
        Position.Write(writer);
    }
}
