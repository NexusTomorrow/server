namespace NexusTomorrow.Maps.Timing;

/// <summary>
/// Names one scheduled timer. The default names no timer, and a handle whose timer fired or was cancelled cancels nothing,
/// even once the wheel reuses its storage.
/// </summary>
/// <param name="Index">The timer's node in the wheel.</param>
/// <param name="Generation">The node's generation when the timer was scheduled; 0 only in the default handle.</param>
public readonly record struct TimerHandle(int Index, int Generation)
{
    /// <summary>
    /// True for the default handle, which names no timer.
    /// </summary>
    public bool IsNone => Generation == 0;
}
