namespace NexusTomorrow.Game.Static.Abilities;

/// <summary>
/// The flags an action set carries. The client tests only bit 0 and gives bit 1 (0x02) no meaning.
/// </summary>
[Flags]
public enum ActionSetFlags
{
    None = 0x00,

    /// <summary>
    /// The action set is unlocked: AbilityBook.GetNumUnlockedSpecs counts it, and the client lets the player switch to it.
    /// </summary>
    Unlocked = 0x01,
}
