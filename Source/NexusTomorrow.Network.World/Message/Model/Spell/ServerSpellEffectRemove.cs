using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellEffectRemove)]
[WireMessage(GameMessageOpcode.ServerSpellEffectRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellEffectRemove : IWritable
{
    [UInt(32)] public uint Spell4EffectId { get; set; }
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(32)] public uint SpellEffectUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4EffectId);
        writer.Write(CastingId);
        writer.Write(SpellEffectUniqueId);
    }
}
