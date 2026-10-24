using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Owners.Account;

namespace NexusTomorrow.Testing;

// The caps a test names, our own fixture numbers: an entitlement it leaves out has no row.
public sealed class FixedEntitlementCaps : IEntitlementCaps
{
    private readonly Dictionary<uint, uint> _caps;

    public FixedEntitlementCaps(Dictionary<uint, uint>? caps = null)
    {
        _caps = caps ?? [];
    }

    public uint? MaxCountOf(uint entitlementId) => _caps.TryGetValue(entitlementId, out uint cap) ? cap : null;

    // The account defaults a test runs with: the options' own defaults unless it names others, against these caps.
    public static AccountDefaults Defaults(AccountDefaultsOptions? options = null, IEntitlementCaps? caps = null) =>
        new(Options.Create(options ?? new AccountDefaultsOptions()), caps ?? new FixedEntitlementCaps(), NullLogger<AccountDefaults>.Instance);
}
