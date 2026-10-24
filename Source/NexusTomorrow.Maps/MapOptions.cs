using NexusTomorrow.Settings;

namespace NexusTomorrow.Maps;

/// <summary>
/// The <c>Maps</c> configuration section: the threads that run maps, and what each map's tick does.
/// </summary>
/// <remarks>
/// <see cref="TickMs"/> times <see cref="ReplicationEveryTicks"/> stays within 25 to 1,000 ms because the client clamps the movement
/// interval it is told to that range (<c>WorldSettings.MovementInterval</c>).
/// </remarks>
[SettingsSection(Section, 4, "Map scheduler", "Map-scheduler#knobs", "Owners", "Owners#knobs")]
[ProductBetween(nameof(TickMs), nameof(ReplicationEveryTicks), 25, 1000, "ms")]
[NotAbove(nameof(AdmitRadius), nameof(DropRadius))]
public sealed class MapOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Maps";

    /// <summary>
    /// How many pool threads run the maps that have no thread of their own; the default leaves three logical processors for the
    /// gateway, the realm and account owners and the garbage collector.
    /// </summary>
    [ComputedDefault("the logical processors less three, at least 1")]
    [AtLeast(1)]
    public int PoolThreads { get; set; } = Math.Max(1, Environment.ProcessorCount - 3);

    /// <summary>
    /// How many maps may run on a thread of their own; a dedicated placement past this many runs in the pool.
    /// </summary>
    [NotNegative]
    public int DedicatedThreads { get; set; }

    /// <summary>
    /// A map's tick period in milliseconds, which divides 250 so an effect timed on a multiple of 250 ms lands exactly on a tick.
    /// </summary>
    /// <remarks>
    /// decisions.md D9 2d.
    /// </remarks>
    [Unit("ms")]
    [OneOf(25, 50, 125, 250)]
    public int TickMs { get; set; } = 25;

    /// <summary>
    /// How many ticks make one replication interval: <see cref="TickMs"/> times this is the interval the client is told to send
    /// its movement at (<c>WorldSettings.MovementInterval</c>), within the client's 25 to 1,000 ms.
    /// </summary>
    public int ReplicationEveryTicks { get; set; } = 5;

    /// <summary>
    /// How often a character that changed is saved in a map; each character's first save falls at random within one interval of
    /// its arrival.
    /// </summary>
    [AtLeastDuration("00:00:01")]
    public TimeSpan SaveInterval { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// How long a tick keeps handling normal-priority messages once it has handled <see cref="NormalDrainFloor"/> of them.
    /// </summary>
    [Positive]
    public TimeSpan NormalDrainBudget { get; set; } = TimeSpan.FromMilliseconds(5);

    /// <summary>
    /// How many normal-priority messages a tick handles at least, however long that takes.
    /// </summary>
    [AtLeast(1)]
    public int NormalDrainFloor { get; set; } = 32;

    /// <summary>
    /// How near, in world units, another player must come before a player sees it: the map creates it for the viewer at its next
    /// replication flush.
    /// </summary>
    [AtLeast(1)]
    public int AdmitRadius { get; set; } = 128;

    /// <summary>
    /// How far, in world units, a seen player may go before the map destroys it for the viewer; beyond <see cref="AdmitRadius"/>, so a
    /// player near the edge is not created and destroyed over and over.
    /// </summary>
    [AtLeast(1)]
    public int DropRadius { get; set; } = 160;

    /// <summary>
    /// The most other players one player sees at once; past it, the nearest are created first.
    /// </summary>
    [AtLeast(1)]
    public int VisibleCap { get; set; } = 256;

    /// <summary>
    /// The most players created for one viewer at one replication flush, so a crowd fills in over a few flushes.
    /// </summary>
    [AtLeast(1)]
    public int SwapsPerCheck { get; set; } = 32;
}
