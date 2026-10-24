using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Owners;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Gateway.Tests;

public sealed class GatedSerializerTests
{
    [Fact]
    public void TheGateRunsOnceAtSerializeNotPerSession()
    {
        var serializer = new GatedSerializer(MessageCatalog.Default);
        var session = new FakeClientSession();

        // UnitDestroyed is read to Purpose and Unknown1 is unmarked, so setting it is refused before any session sees the message.
        InvalidOperationException refused = Assert.Throws<InvalidOperationException>(
            () => session.Send(serializer.Serialize(new UnitDestroyed { UnitId = 1, Unknown1 = true })));

        Assert.Equal("UnitDestroyed.Unknown1 is set, but its meaning is not verified, so the server may not send it.", refused.Message);
        Assert.Empty(session.Sent);
    }

    [Fact]
    public void TheFakeRecordsASharedSendAsTheClientReadsIt()
    {
        var serializer = new FakeMessageSerializer();
        var session = new FakeClientSession();

        SerializedMessage shared = serializer.Serialize(new UnitDestroyed { UnitId = 9 });
        session.Send(shared);
        shared.Release();

        Assert.Equal(9u, session.Single<UnitDestroyed>().UnitId);
        Assert.Equal(0, shared.References);
        Assert.Equal(1, serializer.SerializedCount(typeof(UnitDestroyed)));
    }
}
