namespace NexusTomorrow.Game.Static.Option;

/// <summary>
/// The combat log entry types the player can hide, one bit for each console variable that sets it: the bit named Absorption is
/// cmbtlog.disableAbsorption's, and so on for each. A set bit hides that type's entries, and the client drops them from its combat log.
/// </summary>
[Flags]
public enum CombatLogType
{
    Absorption = 0x0001,
    CCState = 0x0002,
    Damage = 0x0004,
    Deflect = 0x0008,
    DelayDeath = 0x0010,
    Dispel = 0x0020,
    FallingDamage = 0x0040,
    Heal = 0x0080,
    Immunity = 0x0100,
    Interrupted = 0x0200,
    ModifyInterruptArmor = 0x0400,
    Transference = 0x0800,
    VitalModifier = 0x1000,
    Death = 0x2000,
}
