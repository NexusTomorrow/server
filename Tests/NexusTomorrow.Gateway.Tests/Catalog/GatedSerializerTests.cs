using System.Reflection;
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

    [Fact]
    public void NothingButASerializerMakesASerializedMessage()
    {
        // The send gate runs in the serializer, so a message made anywhere else could reach a session ungated.
        Assert.Empty(typeof(SerializedMessage).GetConstructors());
        Type[] makers =
        [
            .. typeof(SerializedMessage).Assembly.GetExportedTypes()
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(method => method.ReturnType == typeof(SerializedMessage))
                .Select(method => method.DeclaringType!),
        ];
        Type[] serializerOnly = [typeof(IMessageSerializer)];
        Assert.Equal(serializerOnly, makers);
    }

    [Fact]
    public void TwoSerializersNeverShareAMessage()
    {
        var first = new GatedSerializer(MessageCatalog.Default);
        var second = new GatedSerializer(MessageCatalog.Default);
        SerializedMessage released = first.Serialize(new UnitDestroyed { UnitId = 1 });
        released.Release();

        SerializedMessage other = second.Serialize(new UnitDestroyed { UnitId = 2 });

        Assert.NotSame(released, other);
        other.Release();
    }

    [Fact]
    public void ABodyReleasedOnAnotherThreadServesTheNextSerializationWithoutAllocating()
    {
        var serializer = new GatedSerializer(MessageCatalog.Default);
        var message = new UnitDestroyed { UnitId = 3 };

        // Each message is released on a thread of its own, which then ends, as a session's writer releases it off the map's tick.
        long allocated = 0;
        for (int i = 0; i < 20; i++)
        {
            long before = GC.GetAllocatedBytesForCurrentThread();
            SerializedMessage shared = serializer.Serialize(message);
            if (i > 0)
                allocated += GC.GetAllocatedBytesForCurrentThread() - before;

            var writer = new Thread(shared.Release);
            writer.Start();
            writer.Join();
        }

        Assert.Equal(0L, allocated);
    }
}
