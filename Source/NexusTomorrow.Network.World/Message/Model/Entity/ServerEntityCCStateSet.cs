using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

// Must be sent same time or after SpellEffects are applied to the unit.
// The SpellEffectUniqueId must match the id from the SpellCast result in ServerSpellGo/ServerSpellExecute
// This triggers the restrictions and VisualEffects for the CC state.
[WireMessage(GameMessageOpcode.ServerEntityCCStateSet, Direction.ServerToClient)]
public sealed partial class ServerEntityCCStateSet
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(5)] public CCState CCType { get; set; }
    [UInt(32)] public uint SpellEffectUniqueId { get; set; }
}
