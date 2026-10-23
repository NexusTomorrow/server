using System.Globalization;
using NexusTomorrow.Settings;

namespace NexusTomorrow.Gateway.Tests;

public sealed class GatewayOptionsTests
{
    [Theory]
    [InlineData("00:00:00", true)]
    [InlineData("00:00:05", true)]
    [InlineData("-00:00:01", false)]
    public void TheDisconnectGraceMayBeZeroButNeverNegative(string grace, bool valid)
    {
        var options = new GatewayOptions { UnknownAccountSecret = Convert.ToBase64String(new byte[32]), DisconnectGrace = TimeSpan.Parse(grace, CultureInfo.InvariantCulture) };

        IReadOnlyList<string> failures = SettingsSection.Of(typeof(GatewayOptions)).Check(options);

        Assert.Equal(valid, failures.Count == 0);
    }
}
