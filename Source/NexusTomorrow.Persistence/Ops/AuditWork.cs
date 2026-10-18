namespace NexusTomorrow.Persistence.Ops;

/// <summary>
/// Appends one row to <c>ops.audit</c>. Audit rows share one key, so they land in the order they were queued.
/// </summary>
public sealed class AuditWork : PersistenceWork
{
    /// <summary>
    /// Create work that appends <paramref name="row"/>.
    /// </summary>
    public AuditWork(AuditRow row)
        : base(PersistKey.Named("audit"))
    {
        Row = row;
    }

    /// <summary>
    /// The row to append.
    /// </summary>
    public AuditRow Row { get; }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        // A fresh context each attempt, so a retry never carries a failed attempt's tracked row.
        await using OpsContext context = contexts.CreateOps();
        context.Audit.Add(Row);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result)
    {
    }
}
