namespace NexusTomorrow.Maps.Scheduling;

/// <summary>
/// Something that ticks at a fixed period, one tick at a time.
/// </summary>
public interface ITickable
{
    /// <summary>
    /// What logs and thread names call this tickable.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The period between ticks, in milliseconds.
    /// </summary>
    int TickMs { get; }

    /// <summary>
    /// Run one tick at the scheduler's clock; the scheduler never runs two at once.
    /// </summary>
    /// <param name="nowMs">The scheduler's clock, in milliseconds, when the tick began.</param>
    void Tick(long nowMs);
}
