namespace NexusTomorrow.Game.Static.Combat.CrowdControl;

/// <summary>
/// The directions of a stun breakout, as the interface's Unit.CodeEnumCCStateStunVictimGameplay names them; a set of
/// direction keys combines them.
/// </summary>
[Flags]
public enum CCStateStunVictimGameplay
{
    None = 0x0,
    Forward = 0x1,
    Backward = 0x2,
    Left = 0x4,
    Right = 0x8
}
