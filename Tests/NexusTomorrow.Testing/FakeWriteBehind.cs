using NexusTomorrow.Persistence;

namespace NexusTomorrow.Testing;

// Keeps queued work for the test to inspect and complete by hand.
public sealed class FakeWriteBehind : IWriteBehind
{
    private readonly Lock _gate = new();
    private readonly List<PersistenceWork> _queued = [];

    public int Backlog
    {
        get
        {
            lock (_gate)
                return _queued.Count;
        }
    }

    public bool IsAvailable { get; set; } = true;

    // When set and true for a work item, Enqueue throws before keeping it: a fault in the caller's handler.
    public Func<PersistenceWork, bool>? ThrowOn { get; set; }

    public List<PersistenceWork> Queued
    {
        get
        {
            lock (_gate)
                return [.. _queued];
        }
    }

    public void Enqueue(PersistenceWork work)
    {
        if (ThrowOn?.Invoke(work) == true)
            throw new InvalidOperationException($"Enqueue of {work.GetType().Name} refused by the test.");

        lock (_gate)
            _queued.Add(work);
    }

    public bool Remove(PersistenceWork work)
    {
        lock (_gate)
            return _queued.Remove(work);
    }

    public T Take<T>()
        where T : PersistenceWork
    {
        lock (_gate)
        {
            T found = _queued.OfType<T>().FirstOrDefault() ?? throw new InvalidOperationException($"No {typeof(T).Name} queued.");
            _queued.Remove(found);
            return found;
        }
    }
}
