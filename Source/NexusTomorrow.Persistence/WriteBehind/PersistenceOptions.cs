using NexusTomorrow.Settings;

namespace NexusTomorrow.Persistence;

/// <summary>
/// The <c>Persistence</c> configuration section.
/// </summary>
/// <remarks>
/// The queue checks none of these, so each limit is a rule the host checks at start.
/// </remarks>
[SettingsSection(Section, 5, "Persistence", "Persistence#knobs")]
[NotAbove(nameof(FirstRetryDelay), nameof(MaxRetryDelay))]
public sealed class PersistenceOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Persistence";

    /// <summary>
    /// The Npgsql connection string.
    /// </summary>
    [Secret]
    [RequiredSetting]
    public required string ConnectionString { get; set; }

    /// <summary>
    /// How many queue workers apply work at once.
    /// </summary>
    /// <remarks>
    /// At least 1: the queue starts none for a lower value.
    /// </remarks>
    [AtLeast(1)]
    public int SaveWorkers { get; set; } = 4;

    /// <summary>
    /// The wait before the first retry of failed work; each retry doubles it.
    /// </summary>
    /// <remarks>
    /// Positive and at most <see cref="MaxRetryDelay"/>: a zero delay retries failing work in a hot loop, and a negative one throws
    /// inside the queue's retry while the work holds its keys (<c>WriteBehindQueue.ApplyUntilDoneAsync</c>).
    /// </remarks>
    [Positive]
    public TimeSpan FirstRetryDelay { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// The longest wait between retries.
    /// </summary>
    /// <remarks>
    /// Positive, as <see cref="FirstRetryDelay"/> is.
    /// </remarks>
    [Positive]
    public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// How long the host's start waits for the first character id block before it fails.
    /// </summary>
    [Positive]
    public TimeSpan FirstIdBlockTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
