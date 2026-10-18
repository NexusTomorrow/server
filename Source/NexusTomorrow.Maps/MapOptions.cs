namespace NexusTomorrow.Maps;

/// <summary>
/// The <c>Maps</c> configuration section: the threads that run maps, and what each map's tick does.
/// </summary>
public sealed class MapOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Maps";

    /// <summary>
    /// How many pool threads run the maps that have no thread of their own; the default leaves three cores for the gateway, the
    /// realm and account owners and the garbage collector.
    /// </summary>
    public int PoolThreads { get; set; } = Math.Max(1, Environment.ProcessorCount - 3);

    /// <summary>
    /// How many maps may run on a thread of their own; a dedicated placement past this many runs in the pool.
    /// </summary>
    public int DedicatedThreads { get; set; }

    /// <summary>
    /// A map's tick period in milliseconds: 25, 50, 125 or 250, so it divides 250 (decisions.md D9 2d).
    /// </summary>
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
    public TimeSpan SaveInterval { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// How long a tick keeps handling normal-priority messages once it has handled <see cref="NormalDrainFloor"/> of them, in
    /// milliseconds.
    /// </summary>
    public int NormalDrainBudget { get; set; } = 5;

    /// <summary>
    /// How many normal-priority messages a tick handles at least, however long that takes.
    /// </summary>
    public int NormalDrainFloor { get; set; } = 32;
}
