namespace NexusTomorrow.Maps.Timing;

/// <summary>
/// Receives the timers a <see cref="TimerWheel{TPayload}"/> fires. The wheel has already removed the timer, so the handler
/// may schedule and cancel timers freely.
/// </summary>
/// <typeparam name="TPayload">The value carried by each timer.</typeparam>
public interface ITimerHandler<TPayload>
    where TPayload : struct
{
    /// <summary>
    /// Handle one fired timer.
    /// </summary>
    /// <param name="payload">The value the timer was scheduled with.</param>
    /// <param name="dueMs">The millisecond the timer was due at.</param>
    void OnTimer(in TPayload payload, long dueMs);
}
