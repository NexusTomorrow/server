using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellEffectExecute, Direction.ServerToClient)]
public sealed partial class ServerSpellEffectExecute
{
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(32)] public uint CasterUnitId { get; set; }
    [UInt(32)] public uint SpellEffectUniqueId { get; set; }
}
