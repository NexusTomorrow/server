namespace NexusTomorrow.Persistence;

/// <summary>
/// How a piece of work ended.
/// </summary>
public enum WorkOutcome
{
    /// <summary>
    /// Committed.
    /// </summary>
    Committed,

    /// <summary>
    /// The database refused it on a unique constraint: the name or email is taken. Nothing was written.
    /// </summary>
    Refused,
}

/// <summary>
/// The outcome of a piece of work and, when refused, the constraint that refused it.
/// </summary>
public readonly record struct WorkResult(WorkOutcome Outcome, string? Constraint)
{
    /// <summary>
    /// A commit.
    /// </summary>
    public static WorkResult Committed { get; } = new(WorkOutcome.Committed, null);

    /// <summary>
    /// A refusal by <paramref name="constraint"/>.
    /// </summary>
    public static WorkResult RefusedBy(string? constraint) => new(WorkOutcome.Refused, constraint);
}
