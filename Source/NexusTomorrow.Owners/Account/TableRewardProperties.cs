using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable;
using NexusTomorrow.GameTable.Model;
using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The account's premium tier and the reward properties it gives, composed once, at construction, by the client's rule
/// (<see cref="RewardPropertyComposition"/>) from the operator's own extracted client tables (<c>RewardPropertyPremiumModifier.tbl</c>
/// and <c>RewardProperty.tbl</c> under <see cref="ContentOptions.ClientDataPath"/>), never from the repository. With no path, or a
/// table missing or unreadable, it logs a warning and holds no values, so world entry sends none.
/// </summary>
public sealed class TableRewardProperties : IRewardProperties
{
    /// <summary>
    /// Compose the values for <paramref name="defaults"/>' tier, a tied row counting the account's entitlements against
    /// <paramref name="caps"/>.
    /// </summary>
    public TableRewardProperties(IOptions<ContentOptions> options, AccountDefaults defaults, IEntitlementCaps caps, ILogger<TableRewardProperties> logger)
    {
        Tier = defaults.PremiumTier;
        Values = Load(options.Value.ClientDataPath, defaults, caps, logger);
    }

    /// <inheritdoc/>
    public byte Tier { get; }

    /// <inheritdoc/>
    public ImmutableArray<RewardPropertyValue> Values { get; }

    private static ImmutableArray<RewardPropertyValue> Load(string clientDataPath, AccountDefaults defaults, IEntitlementCaps caps, ILogger logger)
    {
        if (string.IsNullOrEmpty(clientDataPath))
        {
            AccountLog.NoClientDataPath(logger, "the reward property tables");
            return [];
        }

        string modifiers = Path.Combine(clientDataPath, "tbl", "RewardPropertyPremiumModifier.tbl");
        string properties = Path.Combine(clientDataPath, "tbl", "RewardProperty.tbl");
        if (!File.Exists(modifiers) || !File.Exists(properties))
        {
            AccountLog.RewardTableUnavailable(logger, File.Exists(modifiers) ? properties : modifiers, null);
            return [];
        }

        try
        {
            return RewardPropertyComposition.Compose(
                new GameTable<RewardPropertyPremiumModifierEntry>(modifiers).Entries, new GameTable<RewardPropertyEntry>(properties).Entries,
                defaults.PremiumTier, defaults.CountOf, caps.MaxCountOf, logger);
        }
        catch (Exception exception)
        {
            // Any failure reading the player's tables leaves the values empty and the start going on. GameTable is not hardened
            // against a damaged file, which can fail it with anything from a file the server may not open to a repeated id.
            AccountLog.RewardTableUnavailable(logger, modifiers, exception);
            return [];
        }
    }
}
