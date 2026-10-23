using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Reports one combat event to the combat log and the floaters.
/// </summary>
/// <remarks>
/// The client drops a kind the player's combat log settings hide. For most kinds it raises the event the kind names (CombatLogDamage,
/// CombatLogHeal and the rest) with the entry; <see cref="CombatLogItemDestroyed"/> and <see cref="Server0247Union0Arm22"/> say what
/// becomes of theirs. A payload that names its caster in a CasterId, in place of a <see cref="CombatLogAction"/>, is shown only when the
/// caster is a player or a unit a player owns.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0247, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CombatLog
{
    /// <summary>
    /// The entry's kind, which picks its payload.
    /// </summary>
    [UInt(6)] public byte Type { get; set; }

    /// <summary>
    /// What the entry reports, by its kind.
    /// </summary>
    [Union(nameof(Type))] public ICombatLogPayload Payload { get; set; } = new CombatLogAmount0();
}
