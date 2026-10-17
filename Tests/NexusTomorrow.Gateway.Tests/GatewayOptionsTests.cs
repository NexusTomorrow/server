using System.ComponentModel.DataAnnotations;

namespace NexusTomorrow.Gateway.Tests;

public sealed class GatewayOptionsTests
{
    [Theory]
    [InlineData("00:00:00", true)]
    [InlineData("00:00:05", true)]
    [InlineData("-00:00:01", false)]
    public void TheDisconnectGraceMayBeZeroButNeverNegative(string grace, bool valid)
    {
        var options = new GatewayOptions { UnknownAccountSecret = "x", DisconnectGrace = TimeSpan.Parse(grace, System.Globalization.CultureInfo.InvariantCulture) };

        bool result = Validator.TryValidateObject(options, new ValidationContext(options), [], validateAllProperties: true);

        Assert.Equal(valid, result);
    }
}
