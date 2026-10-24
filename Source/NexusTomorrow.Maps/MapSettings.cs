namespace NexusTomorrow.Maps;

/// <summary>
/// What a map reads from <see cref="MapOptions"/>, plus the realm it belongs to.
/// </summary>
/// <param name="TickMs">The map's tick period in milliseconds.</param>
/// <param name="ReplicationEveryTicks">How many ticks make one replication interval.</param>
/// <param name="SaveInterval">How often a character that changed is saved.</param>
/// <param name="NormalDrainBudget">How long a tick keeps handling normal-priority messages once it has handled the floor.</param>
/// <param name="NormalDrainFloor">How many normal-priority messages a tick handles at least.</param>
/// <param name="RealmId">The realm the map serves.</param>
/// <param name="AdmitRadius">How near another player must come to be seen.</param>
/// <param name="DropRadius">How far a seen player may go before it is no longer seen.</param>
/// <param name="VisibleCap">The most other players one player sees at once.</param>
/// <param name="SwapsPerCheck">The most players created for one viewer at one flush.</param>
public sealed record MapSettings(
    int TickMs, int ReplicationEveryTicks, TimeSpan SaveInterval, TimeSpan NormalDrainBudget, int NormalDrainFloor, ushort RealmId,
    int AdmitRadius = 128, int DropRadius = 160, int VisibleCap = 256, int SwapsPerCheck = 32)
{
    /// <summary>
    /// The interval the client is told to send its movement at, a whole number of ticks.
    /// </summary>
    public uint MovementInterval => (uint)(TickMs * ReplicationEveryTicks);

    /// <summary>
    /// Copy the options a map reads, and the realm id.
    /// </summary>
    public static MapSettings From(MapOptions options, ushort realmId) =>
        new(options.TickMs, options.ReplicationEveryTicks, options.SaveInterval, options.NormalDrainBudget, options.NormalDrainFloor, realmId,
            options.AdmitRadius, options.DropRadius, options.VisibleCap, options.SwapsPerCheck);
}
