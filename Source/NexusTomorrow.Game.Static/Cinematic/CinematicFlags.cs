namespace NexusTomorrow.Game.Static.Cinematic;

/// <summary>
/// The flags the server announces a cinematic with.
/// </summary>
[Flags]
public enum CinematicFlags
{
    ScheduleEnd = 0x0000,

    /// <summary>
    /// Begins a cinematic. Without it, the announcement only queues the end of the cinematic playing.
    /// </summary>
    Begin = 0x0001,

    Unknown2 = 0x0002,

    /// <summary>
    /// Plays the cinematic at once instead of offering the player the prompt; the client reports it accepted.
    /// </summary>
    PlayWithoutPrompt = 0x0004
}
