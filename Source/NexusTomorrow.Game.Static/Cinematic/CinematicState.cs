namespace NexusTomorrow.Game.Static.Cinematic;

/// <summary>
/// A cinematic's progress, as the client reports it to the server.
/// </summary>
public enum CinematicState
{
    /// <summary>
    /// The cinematic is accepted: the player chose to play it, or it played without the prompt.
    /// </summary>
    Accepted = 0,

    /// <summary>
    /// The cinematic finished.
    /// </summary>
    Finished = 1,

    /// <summary>
    /// The cinematic camera started.
    /// </summary>
    CameraStarted = 2,

    /// <summary>
    /// The cinematic camera ended.
    /// </summary>
    CameraEnded = 3,

    /// <summary>
    /// The camera holds after a fade-out and waits for the server to continue the cinematic.
    /// </summary>
    AwaitingContinue = 4
}
