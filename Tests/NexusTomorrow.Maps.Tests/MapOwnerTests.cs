using System.Diagnostics;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class MapOwnerTests : IDisposable
{
    private static readonly TimeSpan s_fiveSeconds = TimeSpan.FromSeconds(5);

    private readonly FakeWriteBehind _persistence = new();
    private readonly MapMetrics _metrics = new();
    private readonly FakeRealmInbox _realm = new();
    private long _now;

    internal static CharacterRecord Record(ulong id) =>
        new(id, 7, "Rebaser Head", 2, 1, 4, 166, 2, 7, 900, new CharacterLocation(1, 3, new Vector3(10, 20, 30), 1.5f));

    // Realm 11 is a number no other fixture value uses, so a realm id taken from the wrong field shows.
    internal static MapSettings Settings() => new(25, 5, TimeSpan.FromHours(1), TimeSpan.FromMilliseconds(5), 32, 11);

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void AnArrivingCharacterIsHeldAndItsSessionRoutedToTheMap()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        Arrive(map, session, 1001);

        Assert.Same(map, session.Map);
        Assert.Equal(1, map.PlayerCount);
        Assert.Empty(_realm.Posted);
    }

    [Fact]
    public void ACharacterForAnEndedSessionGoesStraightBack()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        session.Disconnect("gone");

        Arrive(map, session, 1001);

        CharacterReturned returned = Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted));
        Assert.Equal(1001UL, returned.CharacterId);
        Assert.Equal(Record(1001).Location, returned.Location);
        Assert.Equal(0, _persistence.Backlog);
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void ALeavingCharacterThatMovedIsSavedBeforeTheRealmHearsOfIt()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        var moved = new CharacterLocation(1, 5, new Vector3(4, 5, 6), 1f);
        int savesWhenReturned = -1;
        _realm.OnPost = _ => savesWhenReturned = _persistence.Backlog;
        Arrive(map, session, 1001);
        map.PlayerOf(session)!.MoveTo(moved);

        map.Post(new ClientLeft(session));
        Tick(map);

        Assert.Equal(1, savesWhenReturned);
        Assert.Equal(moved, _persistence.Take<SaveCharacterLocationWork>().Location);
        Assert.Equal(moved, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).Location);
        Assert.Null(session.Map);
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void AnUnchangedCharacterLeavesWithoutASave()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session, 1001);

        map.Post(new ClientLeft(session));
        Tick(map);

        Assert.Equal(0, _persistence.Backlog);
        Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted));
    }

    [Fact]
    public void ACharacterThatChangedIsSavedOnItsTimerAndNotAgainOnceCommitted()
    {
        MapOwner map = NewMap(Settings() with { SaveInterval = TimeSpan.FromSeconds(1) });
        var session = new FakeClientSession();
        Arrive(map, session, 1001);
        map.PlayerOf(session)!.MoveTo(new CharacterLocation(1, 5, new Vector3(1, 1, 1), 0f));

        TickTo(map, 1025);

        SaveCharacterLocationWork save = _persistence.Take<SaveCharacterLocationWork>();
        save.Complete(WorkResult.Committed);
        TickTo(map, 3200);
        Assert.Equal(0, _persistence.Backlog);
    }

    [Fact]
    public void AMapThatOpensFarFromTheClocksStartTicksPromptlyAndSavesWithinOneInterval()
    {
        MapOwner map = NewMap(Settings() with { SaveInterval = TimeSpan.FromSeconds(1) });
        var session = new FakeClientSession();
        _now = 2_592_000_000;
        long start = Stopwatch.GetTimestamp();

        Arrive(map, session, 1001);

        Assert.InRange(Stopwatch.GetElapsedTime(start).TotalMilliseconds, 0, 1000);
        map.PlayerOf(session)!.MoveTo(new CharacterLocation(1, 5, new Vector3(1, 1, 1), 0f));
        TickTo(map, 2_592_001_025);
        Assert.Single(_persistence.Queued.OfType<SaveCharacterLocationWork>());
    }

    [Fact]
    public void HighPriorityMessagesAreHandledBeforeNormalOnes()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        map.Post(new CharacterArrived(session, 7, Record(1001), _realm));
        map.PostHigh(new ClientLeft(session));
        Tick(map);

        Assert.Equal(1, map.PlayerCount);
    }

    [Fact]
    public void TheNormalQueueStopsAtItsBudgetOnceItsFloorIsHandled()
    {
        MapOwner map = NewMap(Settings() with { NormalDrainFloor = 2, NormalDrainBudget = TimeSpan.Zero });
        for (ulong id = 1001; id <= 1005; id++)
            map.Post(new CharacterArrived(new FakeClientSession(), 7, Record(id), _realm));

        Tick(map);

        Assert.Equal(2, map.PlayerCount);
        Assert.Equal(3, map.QueueLength);
    }

    [Fact]
    public void AnArrivalThatThrowsDisconnectsAndStillHandsTheCharacterBack()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession { EnterMapFailure = new InvalidOperationException("the session is broken") };

        Arrive(map, session, 1001);

        Assert.Equal("the map failed handling a message", session.DisconnectReason);
        Assert.Equal(1001UL, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).CharacterId);
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void ASecondCharacterForTheSameSessionGoesStraightBack()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session, 1001);

        Arrive(map, session, 1002);

        Assert.Equal(1002UL, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).CharacterId);
        Assert.Equal(1, map.PlayerCount);
    }

    [Fact]
    public async Task StoppingHandsEveryCharacterBackAndTakesNoMoreMessages()
    {
        MapOwner map = NewMap();
        var first = new FakeClientSession();
        var second = new FakeClientSession();
        Arrive(map, first, 1001);
        Arrive(map, second, 1002);

        Task stopped = map.StopAsync();
        Assert.False(stopped.IsCompleted);
        Tick(map);

        await stopped.WaitAsync(s_fiveSeconds, TestContext.Current.CancellationToken);
        Assert.Equal(2, _realm.Posted.Count);
        Assert.Equal("the map stopped", first.DisconnectReason);
        Assert.Equal("the map stopped", second.DisconnectReason);
        Assert.False(map.Post(new ClientLeft(first)));
    }

    [Fact]
    public async Task AStopHandsBackACharacterWhoseArrivalWasStillQueued()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Assert.True(map.Post(new CharacterArrived(session, 7, Record(1001), _realm)));

        Task stopped = map.StopAsync();
        Tick(map);

        await stopped.WaitAsync(s_fiveSeconds, TestContext.Current.CancellationToken);
        Assert.Equal(1001UL, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).CharacterId);
        Assert.Equal("the map stopped", session.DisconnectReason);
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void ASteadyStateTickAllocatesNothing()
    {
        MapOwner map = NewMap();
        Arrive(map, new FakeClientSession(), 1001);
        Tick(map, 20);

        for (int i = 0; i < 100; i++)
        {
            Tick(map);
            Assert.Equal(0L, map.LastTickAllocatedBytes);
        }
    }

    [Fact]
    public async Task AStopReturnsEveryCharacterWhenASessionThrows()
    {
        MapOwner map = NewMap();
        var broken = new FakeClientSession();
        var sound = new FakeClientSession();
        Arrive(map, broken, 1001);
        Arrive(map, sound, 1002);
        broken.DisconnectFailure = new InvalidOperationException("the session is broken");

        Task stopped = map.StopAsync();
        Tick(map);

        await stopped.WaitAsync(s_fiveSeconds, TestContext.Current.CancellationToken);
        ulong[] returned = [.. _realm.Posted.Cast<CharacterReturned>().Select(r => r.CharacterId).Order()];
        Assert.Equal<ulong[]>([1001, 1002], returned);
        Assert.Equal("the map stopped", sound.DisconnectReason);
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void ALeaveWhoseSessionThrowsReturnsTheCharacterOnce()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        var moved = new CharacterLocation(1, 5, new Vector3(4, 5, 6), 1f);
        int savesWhenReturned = -1;
        _realm.OnPost = _ => savesWhenReturned = _persistence.Backlog;
        Arrive(map, session, 1001);
        map.PlayerOf(session)!.MoveTo(moved);
        session.LeaveMapFailure = new InvalidOperationException("the session is broken");

        map.Post(new ClientLeft(session));
        Tick(map);

        CharacterReturned returned = Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted));
        Assert.Equal(1001UL, returned.CharacterId);
        Assert.Equal(moved, returned.Location);
        Assert.Equal(1, savesWhenReturned);
        Assert.Equal(0, map.PlayerCount);
        Assert.Null(map.PlayerOf(session));
    }

    [Fact]
    public void AHeldArrivalThatFaultsIsHandedBackOnce()
    {
        var logger = new ThrowingLogger(7005);
        var map = new MapOwner(1, Settings(), _persistence, new Random(1), _metrics, logger);
        var session = new FakeClientSession();

        Arrive(map, session, 1001);

        Assert.Equal(1001UL, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).CharacterId);
        Assert.Equal(0, map.PlayerCount);
        Assert.Null(map.PlayerOf(session));
        Assert.Null(session.Map);
        Assert.Equal("the map failed handling a message", session.DisconnectReason);
    }

    [Fact]
    public void AFaultingArrivalForACharacterAnotherSessionHoldsLeavesTheHolderAlone()
    {
        MapOwner map = NewMap();
        var holder = new FakeClientSession();
        var intruder = new FakeClientSession { DisconnectFailure = new InvalidOperationException("the session is broken") };
        Arrive(map, holder, 1001);

        Arrive(map, intruder, 1001);

        Assert.Empty(_realm.Posted);
        Assert.Equal(1, map.PlayerCount);
        Assert.NotNull(map.PlayerOf(holder));
        Assert.Same(map, holder.Map);
    }

    private MapOwner NewMap(MapSettings? settings = null) =>
        new(1, settings ?? Settings(), _persistence, new Random(1), _metrics, NullLogger<MapOwner>.Instance);

    private sealed class ThrowingLogger : ILogger<MapOwner>
    {
        private readonly int _eventId;

        public ThrowingLogger(int eventId) => _eventId = eventId;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (eventId.Id == _eventId)
                throw new InvalidOperationException("the logger is broken");
        }
    }

    private void Tick(MapOwner map, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            _now += 25;
            map.Tick(_now);
        }
    }

    private void TickTo(MapOwner map, long nowMs)
    {
        while (_now < nowMs)
            Tick(map);
    }

    private void Arrive(MapOwner map, FakeClientSession session, ulong characterId)
    {
        map.Post(new CharacterArrived(session, 7, Record(characterId), _realm));
        Tick(map);
    }
}
