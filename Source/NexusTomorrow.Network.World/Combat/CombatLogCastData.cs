using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogCastData
{
    [UInt(32)] public uint CasterId { get; set; }
    [UInt(32)] public uint TargetId { get; set; }
    [UInt(18)] public uint SpellId { get; set; } // 18u
    [UInt(4)] public CombatResult CombatResult { get; set; } // 4u
}
