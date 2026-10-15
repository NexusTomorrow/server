using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellReflect, Direction.ServerToClient)]
public sealed partial class ServerSpellReflect
{
    [UInt(32)] public uint SpellUniqueId { get; set; }
    [UInt(19)] public uint Spell4EffectId { get; set; } = 0;
    [UInt(32)] public uint CasterUnitId { get; set; } = 0;
    [UInt(32)] public uint TargetUnitId { get; set; } = 0;

    public SpellEffectResult SpellEffectResult { get; set; } = new();
}
