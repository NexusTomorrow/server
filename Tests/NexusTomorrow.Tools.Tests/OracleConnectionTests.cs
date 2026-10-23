using System.Threading.Channels;

namespace NexusTomorrow.Tools.Tests;

public sealed class OracleConnectionTests
{
    [Fact]
    public void ALineThatDoesNotFitCallsBackAndIsRefused()
    {
        int calls = 0;
        var lines = Channel.CreateBounded<string>(new BoundedChannelOptions(1) { FullMode = BoundedChannelFullMode.Wait });
        using var connection = new OracleConnection(lines.Writer, full: () => calls++);

        Assert.True(connection.Send("one"));
        Assert.False(connection.Send("two"));
        Assert.Equal(1, calls);
    }

    [Fact]
    public void ATypeThatPutsFieldsAndPropertiesOnTheWireIsRefused()
    {
        Assert.Throws<InvalidOperationException>(() => MessageBinder.WireMembers(typeof(Mixed)));
    }

    private sealed class Mixed
    {
        public int Field = 1;

        public int Property { get; set; }
    }
}
