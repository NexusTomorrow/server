using System.Net;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway.Limits;

namespace NexusTomorrow.Gateway.Tests;

public sealed class ConnectionGuardTests
{
    [Fact]
    public void AnAddressHoldsUpToItsLimitPerListenerAndAReleaseFreesASlot()
    {
        var guard = new ConnectionGuard(Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret(), ConnectionsPerAddress = 2 }));
        var address = IPAddress.Parse("192.0.2.1");
        IDisposable? first = guard.TryAcquire("login", address);
        IDisposable? second = guard.TryAcquire("login", address);
        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Null(guard.TryAcquire("login", address));
        Assert.NotNull(guard.TryAcquire("world", address));
        first!.Dispose();
        Assert.NotNull(guard.TryAcquire("login", address));
    }

    [Fact]
    public void ReleasingTwiceFreesOneSlot()
    {
        var guard = new ConnectionGuard(Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret(), ConnectionsPerAddress = 1 }));
        var address = IPAddress.Parse("192.0.2.1");
        IDisposable lease = guard.TryAcquire("sts", address)!;
        lease.Dispose();
        lease.Dispose();
        Assert.NotNull(guard.TryAcquire("sts", address));
        Assert.Null(guard.TryAcquire("sts", address));
    }

    [Fact]
    public void OneAddressAtItsLimitDoesNotRefuseAnother()
    {
        var guard = new ConnectionGuard(Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret(), ConnectionsPerAddress = 1 }));
        Assert.NotNull(guard.TryAcquire("login", IPAddress.Parse("192.0.2.1")));
        Assert.Null(guard.TryAcquire("login", IPAddress.Parse("192.0.2.1")));
        Assert.NotNull(guard.TryAcquire("login", IPAddress.Parse("192.0.2.2")));
    }

    [Fact]
    public void AnIPv4AddressAndItsMappedFormShareTheLimit()
    {
        var guard = new ConnectionGuard(Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret(), ConnectionsPerAddress = 1 }));
        Assert.NotNull(guard.TryAcquire("login", IPAddress.Parse("192.0.2.1")));
        Assert.Null(guard.TryAcquire("login", IPAddress.Parse("192.0.2.1").MapToIPv6()));
    }

    [Fact]
    public void ALeaseReleasedAfterTheLimitWasHitFreesExactlyItsOwnSlot()
    {
        var guard = new ConnectionGuard(Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret(), ConnectionsPerAddress = 2 }));
        var address = IPAddress.Parse("192.0.2.1");
        IDisposable first = guard.TryAcquire("login", address)!;
        IDisposable second = guard.TryAcquire("login", address)!;
        Assert.Null(guard.TryAcquire("login", address));
        first.Dispose();
        second.Dispose();
        IDisposable? a = guard.TryAcquire("login", address);
        IDisposable? b = guard.TryAcquire("login", address);
        Assert.NotNull(a);
        Assert.NotNull(b);
        Assert.Null(guard.TryAcquire("login", address));
    }
}
