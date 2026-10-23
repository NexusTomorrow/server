namespace NexusTomorrow.Game.Static.Entity;

/// <summary>
/// The ways a unit can be resurrected, as the interface's GameLib.RezType names them; the resurrect dialog's options
/// combine them.
/// </summary>
[Flags]
public enum ResurrectionType
{
    None = 0,
    WakeHere = 1,
    Holocrypt = 2,
    SpellCasterLocation = 4,
    ExitInstance = 32,
    WakeHereServiceToken = 64
}
