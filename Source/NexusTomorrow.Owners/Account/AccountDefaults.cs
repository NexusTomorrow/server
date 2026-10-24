using System.Collections.Frozen;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// What every account holds until accounts store their own (<see cref="AccountDefaultsOptions"/>), resolved once at start-up and read
/// by the account owner, which sends it, and the realm owner, which creates by it. An entitlement with no count is held at its
/// Entitlement row's maxCount, never more, and at 1 when its row sets no cap; one with no count and no row is left out, and logged here
/// once.
/// </summary>
public sealed class AccountDefaults
{
    private readonly FrozenDictionary<uint, uint> _counts;

    /// <summary>
    /// Resolve <paramref name="options"/> against <paramref name="caps"/>.
    /// </summary>
    public AccountDefaults(IOptions<AccountDefaultsOptions> options, IEntitlementCaps caps, ILogger<AccountDefaults> logger)
    {
        AccountDefaultsOptions value = options.Value;
        ImmutableArray<EntitlementCount>.Builder held = ImmutableArray.CreateBuilder<EntitlementCount>();
        foreach (EntitlementGrant grant in value.Held())
        {
            if ((grant.Count ?? CapOf(grant.Id, caps, logger)) is uint count)
                held.Add(new EntitlementCount(grant.Id, count));
        }

        Entitlements = held.ToImmutable();
        _counts = Entitlements.ToFrozenDictionary(entitlement => entitlement.Id, entitlement => entitlement.Count);
        EntitlementIds = _counts.Keys.ToFrozenSet();
        CreationLevelLimit = value.CreationLevelLimit;
        PremiumTier = value.PremiumTier;
    }

    /// <summary>
    /// The entitlements every account holds, in the order the options list them.
    /// </summary>
    public ImmutableArray<EntitlementCount> Entitlements { get; }

    /// <summary>
    /// The ids of <see cref="Entitlements"/>.
    /// </summary>
    public IReadOnlySet<uint> EntitlementIds { get; }

    /// <summary>
    /// The creation level limit every account is sent.
    /// </summary>
    public uint CreationLevelLimit { get; }

    /// <summary>
    /// The premium tier every account holds.
    /// </summary>
    public byte PremiumTier { get; }

    /// <summary>
    /// How many of <paramref name="entitlementId"/> an account holds; 0 when it holds none.
    /// </summary>
    public uint CountOf(uint entitlementId) => _counts.GetValueOrDefault(entitlementId);

    // Null, and logged, when no row names the entitlement; 1 when its row sets no cap.
    private static uint? CapOf(uint entitlementId, IEntitlementCaps caps, ILogger logger)
    {
        uint? cap = caps.MaxCountOf(entitlementId);
        if (cap is null)
            AccountLog.EntitlementWithoutRow(logger, entitlementId);

        return cap == 0 ? 1 : cap;
    }
}
