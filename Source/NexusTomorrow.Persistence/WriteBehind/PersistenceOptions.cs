namespace NexusTomorrow.Persistence;

/// <summary>
/// The <c>Persistence</c> configuration section.
/// </summary>
public sealed class PersistenceOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Persistence";

    /// <summary>
    /// The Npgsql connection string. Required: the queue does not check it, so whatever binds this section validates it.
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Queue workers applying work at once. At least 1: the queue starts none for a lower value, so whatever binds this section
    /// validates it.
    /// </summary>
    public int SaveWorkers { get; set; } = 4;

    /// <summary>
    /// The wait before the first retry of failed work; each retry doubles it.
    /// </summary>
    public TimeSpan FirstRetryDelay { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// The longest wait between retries.
    /// </summary>
    public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// How long the host's start waits for the first character id block before it fails. Positive: whatever binds this section
    /// validates it.
    /// </summary>
    public TimeSpan FirstIdBlockTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
