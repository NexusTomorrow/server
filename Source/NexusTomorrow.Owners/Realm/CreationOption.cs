using NexusTomorrow.Game.Static;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// One character creation option: the race, sex, class and faction it makes, where it starts and what it costs.
/// </summary>
public sealed record CreationOption(
    uint Id, byte Race, byte Sex, byte Class, uint FactionId, bool Enabled, CharacterCreationStart Start, uint Xp, uint Level,
    uint EntitlementRequired, uint CostAmount, bool CostumeOnly)
{
    /// <summary>
    /// True when a player holding <paramref name="accountEntitlements"/> may pick the option, as the client offers it under
    /// <paramref name="creationLevelLimit"/>: it is on, is not costume-only, starts neither at level 50 nor as costume-only, and costs no
    /// currency; and either it needs no entitlement and starts at the limit or below (at 1 or below when the limit is 0,
    /// <c>CreationLevelLimit.Level</c>), or it needs an entitlement the account holds.
    /// </summary>
    public bool IsOfferedTo(IReadOnlySet<uint> accountEntitlements, uint creationLevelLimit) =>
        Enabled && !CostumeOnly && Start is not (CharacterCreationStart.Level50 or CharacterCreationStart.CostumeOnly) && CostAmount == 0
        && (EntitlementRequired == 0 ? Level <= Math.Max(creationLevelLimit, 1) : accountEntitlements.Contains(EntitlementRequired));
}
