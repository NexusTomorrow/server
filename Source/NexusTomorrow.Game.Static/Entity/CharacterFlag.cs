namespace NexusTomorrow.Game.Static.Entity;

/// <summary>
/// The character's settings that the server keeps for it: the friendship block, ignoring duel requests and the guild holomark's display.
/// </summary>
[Flags]
public enum CharacterFlag
{
    None = 0x0000,

    /// <summary>
    /// The friendship block, which FriendshipLib.IsLocked reports.
    /// </summary>
    FriendBlock = 0x0002,

    /// <summary>
    /// Ignoring duel requests, which GameLib.IsIgnoringDuelRequests reports.
    /// </summary>
    IgnoreDuelRequests = 0x0008,

    /// <summary>
    /// Hides the guild holomark GameLib.GuildHolomark names Left: GameLib.GetGuildHolomarkVisible reports it hidden.
    /// </summary>
    HolomarkHideLeft = 0x0020,

    /// <summary>
    /// Hides the guild holomark GameLib.GuildHolomark names Right: GameLib.GetGuildHolomarkVisible reports it hidden.
    /// </summary>
    HolomarkHideRight = 0x0040,

    /// <summary>
    /// Hides the guild holomark GameLib.GuildHolomark names Back: GameLib.GetGuildHolomarkVisible reports it hidden.
    /// </summary>
    HolomarkHideBack = 0x0080,

    /// <summary>
    /// The guild holomark's distance setting, which GameLib.GetGuildHolomarkDistance reports.
    /// </summary>
    HolomarkNear = 0x0200,
}
