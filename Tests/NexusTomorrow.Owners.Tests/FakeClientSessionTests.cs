using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class FakeClientSessionTests
{
    [Fact]
    public void AMessageChangedAfterSendKeepsTheValueItHadWhenSent()
    {
        var session = new FakeClientSession();
        var message = new LoginAccepted { DroppedForLag = 1 };

        session.Send(message);
        message.DroppedForLag = 2;
        session.Send(message);

        uint[] expected = [1, 2];
        Assert.Equal(expected, session.Sent.Cast<LoginAccepted>().Select(sent => sent.DroppedForLag));
    }
}
