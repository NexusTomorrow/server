namespace NexusTomorrow.Game.Static.Cinematic;

/// <summary>
/// How a cinematic transition moves to its new view; the camera's transitions and the screen fade take the same values.
/// </summary>
public enum ScaleTransitionType
{
    /// <summary>
    /// On a camera transition, cuts to the new view at once.
    /// </summary>
    Cut = 0,

    /// <summary>
    /// On a camera transition, blends to the new view over the transition's duration.
    /// </summary>
    Blend = 1,

    /// <summary>
    /// Fades the screen out to a set color over the duration, holds it, then fades it back in.
    /// </summary>
    FadeColor = 2,

    /// <summary>
    /// Fades the screen out to white over the duration, holds it, then fades it back in.
    /// </summary>
    FadeWhite = 3
}
