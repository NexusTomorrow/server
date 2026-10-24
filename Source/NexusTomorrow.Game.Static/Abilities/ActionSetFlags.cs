namespace NexusTomorrow.Game.Static.Abilities;

/// <summary>
/// The flags an action set carries, which the client tests bit by bit.
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
