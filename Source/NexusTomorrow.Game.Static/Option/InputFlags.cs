namespace NexusTomorrow.Game.Static.Option;

/// <summary>
/// The player's casting options, one bit for each console variable that sets it. The client also sets bit 0x1 at start and never clears
/// it, so every mask it sends carries that bit, which has no meaning.
/// </summary>
[Flags]
public enum InputFlags
{
    /// <summary>
    /// Set while the console variable spell.useButtonDownForAbilities is on.
    /// </summary>
    UseButtonDownForAbilities = 0x2,

    /// <summary>
    /// Set while the console variable spell.disableAutoTargeting is on.
    /// </summary>
    DisableAutoTargeting = 0x4,

    /// <summary>
    /// Set while the console variable spell.holdToContinueCasting is on.
    /// </summary>
    HoldToContinueCasting = 0x8,
}
