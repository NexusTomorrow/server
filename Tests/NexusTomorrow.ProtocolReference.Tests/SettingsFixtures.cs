using NexusTomorrow.Settings;

namespace NexusTomorrow.ProtocolReference.Tests;

/// <summary>
/// The Sample section, explained by two pages.
/// </summary>
[SettingsSection("Sample", 1, "Map scheduler", "Map-scheduler#knobs", "Owners", "Owners#knobs")]
[ProductBetween(nameof(TickMs), nameof(BatchTicks), 25, 1000, "ms")]
public sealed class SampleSettings
{
    /// <summary>
    /// The connection the sample opens.
    /// </summary>
    [Secret]
    [RequiredSetting]
    public required string ConnectionString { get; set; }

    /// <summary>
    /// How many workers run the samples; the default leaves two processors free.
    /// </summary>
    [ComputedDefault("the logical processors less two, at least 1")]
    [AtLeast(1)]
    public int Workers { get; set; } = Math.Max(1, Environment.ProcessorCount - 2);

    /// <summary>
    /// The sample's id.
    /// </summary>
    [Between(1, 16383)]
    public ushort Id { get; set; } = 1;

    /// <summary>
    /// A sample tick in milliseconds.
    /// </summary>
    [Unit("ms")]
    [OneOf(25, 50, 125, 250)]
    public int TickMs { get; set; } = 25;

    /// <summary>
    /// How many ticks make one batch.
    /// </summary>
    public int BatchTicks { get; set; } = 5;
}

/// <summary>
/// The Quiet section, explained by one page, with no rules.
/// </summary>
[SettingsSection("Quiet", 2, "Command layer", "Command-layer#knobs")]
public sealed class QuietSettings
{
    /// <summary>
    /// Whether the sample console runs.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// How long the sample waits.
    /// </summary>
    public TimeSpan Wait { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Where the sample reads from.
    /// </summary>
    public string Folder { get; set; } = "";
}
