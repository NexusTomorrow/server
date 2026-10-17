namespace NexusTomorrow.Persistence;

/// <summary>
/// Where owners hand their database work. Safe to call from any thread.
/// </summary>
public interface IWriteBehind
{
    /// <summary>
    /// The work queued or running.
    /// </summary>
    int Backlog { get; }

    /// <summary>
    /// False while the last attempt to reach the database failed to connect; logins are refused then.
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// Queue <paramref name="work"/> behind earlier work on any of its keys.
    /// </summary>
    void Enqueue(PersistenceWork work);
}
