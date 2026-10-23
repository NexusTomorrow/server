namespace NexusTomorrow.Tools.Tests;

public sealed class OracleOptionsValidatorTests
{
    private static readonly OracleOptionsValidator s_validator = new();

    [Fact]
    public void AnEndpointThatIsOffIsNeverRefused()
    {
        var options = new OracleOptions { Enabled = false, Address = "0.0.0.0", Port = 0, TokenFile = "" };
        Assert.True(s_validator.Validate(null, options).Succeeded);
    }

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("::1")]
    public void AnEnabledEndpointOnALoopbackAddressPasses(string address)
    {
        var options = new OracleOptions { Enabled = true, Address = address, Port = 23900, TokenFile = Path.GetFullPath("token") };
        Assert.True(s_validator.Validate(null, options).Succeeded);
    }

    [Theory]
    [InlineData("0.0.0.0")]
    [InlineData("192.168.1.10")]
    [InlineData("localhost")]
    [InlineData("")]
    public void AnEnabledEndpointOffLoopbackIsRefused(string address)
    {
        var options = new OracleOptions { Enabled = true, Address = address, Port = 23900, TokenFile = Path.GetFullPath("token") };
        string failure = Assert.Single(s_validator.Validate(null, options).Failures!);
        Assert.Contains("Address must be a loopback address", failure, StringComparison.Ordinal);
    }

    [Fact]
    public void APortOutOfRangeAndARelativeTokenFileAreRefused()
    {
        var options = new OracleOptions { Enabled = true, Address = "127.0.0.1", Port = 70000, TokenFile = "token" };
        Assert.Equal(2, s_validator.Validate(null, options).Failures!.Count());
    }
}
