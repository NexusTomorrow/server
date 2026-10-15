using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellEffectResultAnnounce, Direction.ServerToClient)]
public sealed partial class ServerSpellEffectResultAnnounce
{
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(19)] public uint Spell4EffectId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }

    [Array(CountBits = 8)] public List<SpellEffectResult> SpellEffectResults { get; set; } = [];
}
