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
    /// True when a player may pick the option: it is on, is not costume-only, does not start at level 50 or as costume-only, and
    /// needs no entitlement or currency (Stage 1 has neither).
    /// </summary>
    public bool IsOffered =>
        Enabled && !CostumeOnly && Start is not (CharacterCreationStart.Level50 or CharacterCreationStart.CostumeOnly)
        && EntitlementRequired == 0 && CostAmount == 0;
}
