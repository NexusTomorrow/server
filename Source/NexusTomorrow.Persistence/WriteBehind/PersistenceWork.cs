using System.Collections.Immutable;

namespace NexusTomorrow.Persistence;

/// <summary>
/// One unit of database work an owner hands to the write-behind queue: a change set or a load. It names every key it touches, runs on
/// a queue worker, and reports back through <see cref="Complete"/>, whose implementation posts a message to its owner.
/// </summary>
public abstract class PersistenceWork
{
    /// <summary>
    /// Create work over <paramref name="keys"/>, which must be distinct and at least one.
    /// </summary>
    protected PersistenceWork(params ReadOnlySpan<PersistKey> keys)
    {
        if (keys.IsEmpty)
            throw new ArgumentException("Work touches at least one key.", nameof(keys));
        ImmutableArray<PersistKey> distinct = [.. new HashSet<PersistKey>(keys.ToArray())];
        if (distinct.Length != keys.Length)
            throw new ArgumentException("Work names each key once.", nameof(keys));
        Keys = [.. keys];
    }

    /// <summary>
    /// The keys this work touches.
    /// </summary>
    public ImmutableArray<PersistKey> Keys { get; }

    /// <summary>
    /// Do the work, with one <c>SaveChanges</c> for a change set. Called again after a failure, so it must be repeatable.
    /// </summary>
    public abstract Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken);

    /// <summary>
    /// Fold <paramref name="newer"/>, queued later on this work's single key, into this waiting work, and take over its completion.
    /// Only a single-key snapshot can merge; the default refuses.
    /// </summary>
    public virtual bool TryMerge(PersistenceWork newer) => false;

    /// <summary>
    /// Report the outcome, once, on a queue worker's thread. Must not block.
    /// </summary>
    public abstract void Complete(WorkResult result);
}
