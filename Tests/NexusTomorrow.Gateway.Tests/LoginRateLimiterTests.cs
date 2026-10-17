using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Limits;

namespace NexusTomorrow.Gateway.Tests;

public sealed class LoginRateLimiterTests
{
    [Fact]
    public void AnAddressGetsItsAttemptsPerMinuteThenWaits()
    {
        var time = new FakeTimeProvider();
        var limiter = new LoginRateLimiter(time, Options.Create(new GatewayOptions { UnknownAccountSecret = Secret(), LoginAttemptsPerMinute = 2 }));
        var address = IPAddress.Parse("192.0.2.1");
        Assert.True(limiter.TryAttempt(address));
        Assert.True(limiter.TryAttempt(address));
        Assert.False(limiter.TryAttempt(address));
        Assert.True(limiter.TryAttempt(IPAddress.Parse("192.0.2.2")));
        time.Advance(TimeSpan.FromSeconds(61));
        Assert.True(limiter.TryAttempt(address));
    }

    [Fact]
    public void ARefusedAttemptDoesNotExtendTheWait()
    {
        var time = new FakeTimeProvider();
        var limiter = new LoginRateLimiter(time, Options.Create(new GatewayOptions { UnknownAccountSecret = Secret(), LoginAttemptsPerMinute = 1 }));
        var address = IPAddress.Parse("192.0.2.1");
        Assert.True(limiter.TryAttempt(address));
        time.Advance(TimeSpan.FromSeconds(40));
        Assert.False(limiter.TryAttempt(address));
        time.Advance(TimeSpan.FromSeconds(21));
        Assert.True(limiter.TryAttempt(address));
    }

    [Fact]
    public void AddressesThatNeverReturnAreForgotten()
    {
        var time = new FakeTimeProvider();
        var limiter = new LoginRateLimiter(time, Options.Create(new GatewayOptions { UnknownAccountSecret = Secret() }));
        for (int host = 1; host <= 3; host++)
            limiter.TryAttempt(IPAddress.Parse($"192.0.2.{host}"));
        Assert.Equal(3, limiter.TrackedAddresses);
        time.Advance(TimeSpan.FromSeconds(61));
        limiter.TryAttempt(IPAddress.Parse("192.0.2.9"));
        Assert.Equal(1, limiter.TrackedAddresses);
    }

    [Fact]
    public void AnIPv4AddressAndItsMappedFormShareOneAllowance()
    {
        var limiter = new LoginRateLimiter(new FakeTimeProvider(), Options.Create(new GatewayOptions { UnknownAccountSecret = Secret(), LoginAttemptsPerMinute = 1 }));
        Assert.True(limiter.TryAttempt(IPAddress.Parse("192.0.2.1")));
        Assert.False(limiter.TryAttempt(IPAddress.Parse("::ffff:192.0.2.1")));
    }

    internal static string Secret() => Convert.ToBase64String(new byte[32]);
}
