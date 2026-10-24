using Microsoft.Extensions.Options;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

// Entitlements 900 to 902 and their caps are our own fixture numbers.
public sealed class AccountDefaultsTests
{
    [Fact]
    public void ALicenseGoesAtItsCapAndNeverMore()
    {
        AccountDefaults defaults = FixedEntitlementCaps.Defaults(Granting(900), new FixedEntitlementCaps(new() { [900] = 4 }));

        Assert.Equal(new EntitlementCount(900, 4), Assert.Single(defaults.Entitlements));
    }

    [Fact]
    public void ALicenseWithNoRowIsLeftOutAndLogged()
    {
        var logger = new RecordingLogger<AccountDefaults>();

        var defaults = new AccountDefaults(Options.Create(Granting(901)), new FixedEntitlementCaps(), logger);

        Assert.Empty(defaults.Entitlements);
        Assert.StartsWith("Warning", Assert.Single(logger.Entries), StringComparison.Ordinal);
    }

    [Fact]
    public void ALicenseWhoseRowSetsNoCapIsHeldOnce()
    {
        AccountDefaults defaults = FixedEntitlementCaps.Defaults(Granting(902), new FixedEntitlementCaps(new() { [902] = 0 }));

        Assert.Equal(new EntitlementCount(902, 1), Assert.Single(defaults.Entitlements));
    }

    [Fact]
    public void TheFullGameHoldsTheSignatureTierAndEveryLicenseAtItsCap()
    {
        // Every license capped at 5 here, a fixture figure.
        uint[] licenses = [12, 3, 27, 23, 25, 26, 36, 71, 74, 75, 42, 43];
        var caps = new FixedEntitlementCaps(licenses.ToDictionary(id => id, _ => 5u));

        AccountDefaults defaults = FixedEntitlementCaps.Defaults(caps: caps);

        EntitlementCount[] expected =
        [
            .. new uint[] { 4, 10, 14, 15, 16, 17, 18, 19, 20, 21, 22, 61, 62 }.Select(id => new EntitlementCount(id, 1)),
            .. licenses.Select(id => new EntitlementCount(id, 5)),
        ];
        Assert.Equal(expected, defaults.Entitlements);
        Assert.Equal((byte)1, defaults.PremiumTier);
    }

    private static AccountDefaultsOptions Granting(uint entitlement) => new() { Entitlements = [new EntitlementGrant { Id = entitlement }] };
}
