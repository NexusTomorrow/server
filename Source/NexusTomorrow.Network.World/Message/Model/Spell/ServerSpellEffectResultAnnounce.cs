using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellEffectResultAnnounce)]
[WireMessage(GameMessageOpcode.ServerSpellEffectResultAnnounce, Direction.ServerToClient)]
public sealed partial class ServerSpellEffectResultAnnounce : IWritable
{
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(19)] public uint Spell4EffectId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }

    [Array(CountBits = 8)] public List<SpellEffectResult> SpellEffectResults { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CastingId);
        writer.Write(Spell4EffectId, 19);
        writer.Write(TargetUnitId);

        writer.Write(SpellEffectResults.Count, 8u);
        SpellEffectResults.ForEach(u => u.Write(writer));
    }
}
