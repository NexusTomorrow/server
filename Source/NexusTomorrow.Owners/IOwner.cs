namespace NexusTomorrow.Owners;

/// <summary>
/// What the watchdog and the console read from an owner, from any thread.
/// </summary>
public interface IOwner
{
    /// <summary>
    /// The owner's name, for logs and the console.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The messages waiting in the owner's queue.
    /// </summary>
    int QueueLength { get; }

    /// <summary>
    /// The messages the owner has handled since it started; for a map, the ticks it has completed.
    /// </summary>
    long Handled { get; }
}
