using System.Collections.Immutable;
using NexusTomorrow.Owners.Account;

namespace NexusTomorrow.Maps;

/// <summary>
/// What a map reads from <see cref="MapOptions"/>, plus the realm it belongs to (its id and its name) and what every account enters
/// the world with (<see cref="PremiumTier"/>, <see cref="RewardProperties"/>).
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
/// <param name="SayRange">How near a player must stand to hear a <c>/say</c> line.</param>
/// <param name="RealmName">The realm's name, which a chat line's sender is told it was said on.</param>
public sealed record MapSettings(
    int TickMs, int ReplicationEveryTicks, TimeSpan SaveInterval, TimeSpan NormalDrainBudget, int NormalDrainFloor, ushort RealmId,
    int AdmitRadius = 128, int DropRadius = 160, int VisibleCap = 256, int SwapsPerCheck = 32, int SayRange = 155, string RealmName = "")
{
    /// <summary>
    /// The interval the client is told to send its movement at, a whole number of ticks.
    /// </summary>
    public uint MovementInterval => (uint)(TickMs * ReplicationEveryTicks);

    /// <summary>
    /// The premium tier every account holds, carried on every player's unit.
    /// </summary>
    public byte PremiumTier { get; init; }

    /// <summary>
    /// The reward properties that tier gives, which world entry ends with; none sends none.
    /// </summary>
    public ImmutableArray<RewardPropertyValue> RewardProperties { get; init; } = [];

    /// <summary>
    /// Copy the options a map reads, and the realm's id and name.
    /// </summary>
    public static MapSettings From(MapOptions options, ushort realmId, string realmName) =>
        new(options.TickMs, options.ReplicationEveryTicks, options.SaveInterval, options.NormalDrainBudget, options.NormalDrainFloor, realmId,
            options.AdmitRadius, options.DropRadius, options.VisibleCap, options.SwapsPerCheck, options.SayRange, realmName);
}
