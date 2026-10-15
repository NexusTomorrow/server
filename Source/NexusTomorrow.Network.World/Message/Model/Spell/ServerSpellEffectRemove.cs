using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellEffectRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellEffectRemove
{
    [UInt(32)] public uint Spell4EffectId { get; set; }
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(32)] public uint SpellEffectUniqueId { get; set; }
}
