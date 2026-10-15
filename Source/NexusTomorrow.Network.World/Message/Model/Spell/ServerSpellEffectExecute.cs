using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellEffectExecute)]
[WireMessage(GameMessageOpcode.ServerSpellEffectExecute, Direction.ServerToClient)]
public sealed partial class ServerSpellEffectExecute : IWritable
{
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(32)] public uint CasterUnitId { get; set; }
    [UInt(32)] public uint SpellEffectUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CastingId);
        writer.Write(CasterUnitId);
        writer.Write(SpellEffectUniqueId);
    }
}
