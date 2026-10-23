namespace NexusTomorrow.Game.Static.Entity;

/// <summary>
/// Enum containing properties used to set the <see cref="Player"/> PvP Flag
/// </summary>
[Flags]
public enum PvPFlag
{
    Disabled = 0x00,
    Enabled = 0x01,
    Forced = 0x02  // The player cannot turn PvP on or off from the portrait menu.
}
