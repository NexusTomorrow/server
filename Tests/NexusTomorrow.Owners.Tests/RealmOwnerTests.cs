using System.Collections.Immutable;
using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.Text;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class RealmOwnerTests : IAsyncLifetime
{
    private const long Account = 7;

    // Made-up creation options: our own fixture values, not table rows.
    private static readonly FixedCreationCatalog s_catalog = new(
    [
        new CreationOption(1, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: true, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(2, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: false, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(3, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: true, CharacterCreationStart.Level50, Xp: 0, Level: 50, EntitlementRequired: 0, CostAmount: 1, CostumeOnly: false),
    ]);

    // A made-up word, never one of the client's.
    private static readonly WordFilter s_filter = new([(0, "blorp")], NullLogger<WordFilter>.Instance);

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly FakeWriteBehind _db = new();
    private readonly FakeClientSession _session = new();
    private readonly FakeMapDirectory _maps = new();
    private readonly RealmOwner _owner;

    public RealmOwnerTests()
    {
        _owner = new RealmOwner(_db, s_catalog, s_filter, _maps, _time, Options.Create(new RealmOptions { MaxCharactersPerAccount = 2 }), NullLogger<RealmOwner>.Instance);
    }

    public ValueTask InitializeAsync()
    {
        _owner.Post(new IdBlockTaken(new IdBlock(1001, 1000)));
        _owner.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync() => await _owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));

    [Fact]
    public async Task AListLoadsOnceAndAnswersFromTheCache()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        CharacterList list = _session.Single<CharacterList>();
        Assert.Equal("Rebaser Head", Assert.Single(list.Characters).Name);
        Assert.Equal((ulong)_time.GetUtcNow().ToFileTime(), list.ServerTime);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Empty(_db.Queued.OfType<LoadCharactersWork>());
    }

    [Fact]
    public async Task ACreateIsAnsweredAfterItsCommitWithResult3()
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterCreateResult>());

        _db.Take<CreateCharacterWork>().Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        CharacterCreateResult result = _session.Single<CharacterCreateResult>();
        Assert.Equal(CreateResults.Created, result.Result);
        Assert.Equal(1001UL, result.CharacterId);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Equal("Rebaser Head", Assert.Single(_session.Sent.OfType<CharacterList>().Last().Characters).Name);
    }

    [Fact]
    public async Task ANameTheDatabaseRefusesAnswersResult6()
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        _db.Take<CreateCharacterWork>().Complete(WorkResult.RefusedBy(RealmContext.LiveNameIndex));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.NameTaken, _session.Single<CharacterCreateResult>().Result);
    }

    [Fact]
    public async Task ANamePendingInAnyCaseAnswersResult6()
    {
        var other = new FakeClientSession();
        await ListAsync();
        await ListAsync(other, accountId: 8);
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        _owner.Post(new CreateCharacter(8, other, Create("rebaser HEAD")));
        await Eventually.TrueAsync(() => other.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.NameTaken, other.Single<CharacterCreateResult>().Result);
        Assert.Single(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Theory]
    [InlineData("Rebaserhead", 10)]
    [InlineData("Rebaser H3ad", 10)]
    public async Task ANameOutsideTheRulesAnswersResult10(string name, int expected)
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create(name)));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(expected, _session.Single<CharacterCreateResult>().Result);
        Assert.Empty(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Fact]
    public async Task ACreateWithAFilteredNameIsRefusedAsNotAllowed()
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create("Blorp Smith")));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.NameNotAllowed, _session.Single<CharacterCreateResult>().Result);
        Assert.Empty(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Fact]
    public async Task AFullAccountAnswersResult9()
    {
        await ListAsync(Summary(5, "Aaaa Bbbb"), Summary(6, "Cccc Dddd"));
        _owner.Post(new CreateCharacter(Account, _session, Create("Eeee Ffff")));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.AccountFull, _session.Single<CharacterCreateResult>().Result);
    }

    [Theory]
    [InlineData(2u)]  // switched off
    [InlineData(3u)]  // a level-50 start with a cost
    [InlineData(99u)] // not in the catalog
    public async Task AnOptionNotOfferedAnswersTheGeneralFailure(uint creationId)
    {
        await ListAsync();
        CharacterCreate request = Create("Rebaser Head");
        request.CreationId = creationId;
        _owner.Post(new CreateCharacter(Account, _session, request));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.General, _session.Single<CharacterCreateResult>().Result);
    }

    [Fact]
    public async Task ADeleteIsAnsweredAfterItsCommitAndTheListForgetsIt()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new DeleteCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<DeleteCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterDeleteResult>());

        _db.Take<DeleteCharacterWork>().Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterDeleteResult>().Any());
        Assert.Equal(DeleteResults.Deleted, _session.Single<CharacterDeleteResult>().Result);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Empty(_session.Sent.OfType<CharacterList>().Last().Characters);
    }

    [Fact]
    public async Task DeletingAnotherAccountsCharacterAnswersTheGeneralFailure()
    {
        await ListAsync();
        _owner.Post(new DeleteCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterDeleteResult>().Any());
        Assert.Equal(DeleteResults.General, _session.Single<CharacterDeleteResult>().Result);
    }

    [Fact]
    public async Task ARenameNeedsTheRenameMark()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new RenameCharacter(Account, _session, 5, "Other Name"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(RenameResults.NoRenameAvailable, _session.Single<CharacterRenameResult>().Result);
    }

    [Fact]
    public async Task AMarkedCharacterIsRenamedAfterTheCommit()
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = true });
        _owner.Post(new RenameCharacter(Account, _session, 5, "Other Name"));
        await Eventually.TrueAsync(() => _db.Queued.OfType<RenameCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterRenameResult>());

        _db.Take<RenameCharacterWork>().Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        CharacterRenameResult result = _session.Single<CharacterRenameResult>();
        Assert.Equal(RenameResults.Renamed, result.Result);
        Assert.Equal("Other Name", result.Name);
        Assert.Equal(5UL, result.CharacterId);
    }

    [Theory]
    [InlineData(true, false, 1)]
    [InlineData(false, true, 3)]
    public async Task ASelectionAnswersSelectFailedWithItsReason(bool requiresRename, bool disabled, int reason)
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = requiresRename, Disabled = disabled });
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(reason, _session.Single<CharacterSelectFailed>().Unknown0);
    }

    [Fact]
    public async Task ASelectLoadsTheCharacterAndHandsItToItsWorldsMap()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        PersistKey[] expectedKeys = [PersistKey.Character(5)];
        Assert.Equal(expectedKeys, load.Keys.ToArray());

        load.Character = Record(5, 1);
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);
        CharacterArrived arrived = Assert.IsType<CharacterArrived>(map.Posted[0]);
        Assert.Same(_session, arrived.Session);
        Assert.Equal(Account, arrived.AccountId);
        Assert.Equal(5UL, arrived.Character.Id);
        Assert.Same(_owner, arrived.ReturnTo);
        Assert.Empty(_session.Sent.OfType<CharacterSelectFailed>());
    }

    [Fact]
    public async Task ASelectForAWorldNoMapHostsFailsWithTheGeneralReasonAndFreesTheCharacter()
    {
        await ListAsync(InWorld(5, "Rebaser Head", 9));
        await SelectAndLoadAsync(_session, 5, Record(5, 9));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(SelectReasons.General, _session.Single<CharacterSelectFailed>().Unknown0);

        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
    }

    [Fact]
    public async Task ASelectWhoseMapRefusesTheArrivalFailsWithTheGeneralReasonAndFreesTheCharacter()
    {
        FakeMapInbox map = _maps.Add(1);
        map.Accepting = false;
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await SelectAndLoadAsync(_session, 5, Record(5, 1));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(SelectReasons.General, _session.Single<CharacterSelectFailed>().Unknown0);
        Assert.Empty(map.Posted);

        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
    }

    [Fact]
    public async Task ACharacterTheLoadCannotFindFailsWithTheGeneralReason()
    {
        _maps.Add(1);
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await SelectAndLoadAsync(_session, 5, null);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(SelectReasons.General, _session.Single<CharacterSelectFailed>().Unknown0);
    }

    [Fact]
    public async Task ASecondSelectFromASessionWithACharacterOnItsWayChangesNothing()
    {
        _maps.Add(1);
        await ListAsync(InWorld(5, "Aaaa Bbbb", 1), InWorld(6, "Cccc Dddd", 1));
        _owner.Post(new SelectCharacter(Account, _session, 5));
        _owner.Post(new SelectCharacter(Account, _session, 6));
        await SyncAsync(_owner, _session);

        Assert.Single(_db.Queued.OfType<LoadCharacterWork>());
        Assert.Empty(_session.Sent.OfType<CharacterSelectFailed>());
    }

    [Fact]
    public async Task ASelectOfACharacterAnotherSessionHoldsWaitsForItsReturn()
    {
        FakeMapInbox map = _maps.Add(1);
        var other = new FakeClientSession();
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await EnterAsync(_session, 5, map);

        await ListAsync(other, Account, InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, other, 5));
        await SyncAsync(_owner, other);
        Assert.Empty(_db.Queued.OfType<LoadCharacterWork>());

        _owner.Post(new CharacterReturned(Account, 5, new CharacterLocation(1, 4, new Vector3(7, 8, 9), 0.5f)));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        load.Character = Record(5, 1);
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => map.Posted.Count == 2);
        CharacterArrived arrived = Assert.IsType<CharacterArrived>(map.Posted[1]);
        Assert.Same(other, arrived.Session);
    }

    [Fact]
    public async Task AReturnedCharacterIsListedWhereItLeftTheWorld()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await EnterAsync(_session, 5, map);

        _owner.Post(new CharacterReturned(Account, 5, new CharacterLocation(1, 4, new Vector3(7, 8, 9), 0.5f)));
        await SyncAsync(_owner, _session);

        CharacterList.Character listed = Assert.Single(_session.Sent.OfType<CharacterList>().Last().Characters);
        Assert.Equal((ushort)1, listed.WorldId);
        Assert.Equal((ushort)4, listed.ZoneId);
        Assert.Equal(new Vector3(7, 8, 9), listed.Position);
    }

    [Fact]
    public async Task ALoadThatLandsAfterItsSessionClosedIsDroppedAndFreesTheCharacter()
    {
        FakeMapInbox map = _maps.Add(1);
        var other = new FakeClientSession();
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        _owner.Post(new RealmSessionClosed(Account, _session));
        load.Character = Record(5, 1);
        load.Complete(WorkResult.Committed);

        await ListAsync(other, Account, InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, other, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        Assert.Empty(map.Posted);
    }

    [Fact]
    public async Task ACharacterInTheWorldCannotBeDeleted()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await EnterAsync(_session, 5, map);

        _owner.Post(new DeleteCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterDeleteResult>().Any());
        Assert.Equal(DeleteResults.General, _session.Single<CharacterDeleteResult>().Result);
        Assert.Empty(_db.Queued.OfType<DeleteCharacterWork>());
    }

    [Fact]
    public async Task ACharacterLoadedFaultAfterTheArrivalWasPostedLeavesTheCharacterHeld()
    {
        FakeMapInbox map = _maps.Add(1);
        var db = new FakeWriteBehind();
        RealmOwner owner = StartOwner(db, 501);
        try
        {
            await ListOnAsync(owner, db, _session, Account, InWorld(5, "Rebaser Head", 1));

            // The second create takes the allocator below its refill mark and then throws, so the refill is still owed when the
            // next message ends; that message is the load, and its refill throws after the arrival is posted.
            int refills = 0;
            db.ThrowOn = work =>
            {
                if (work is TakeIdBlockWork)
                    Interlocked.Increment(ref refills);

                return work is CreateCharacterWork or TakeIdBlockWork;
            };
            owner.Post(new CreateCharacter(Account, _session, Create("Aaaa Bbbb")));
            owner.Post(new SelectCharacter(Account, _session, 5));
            await Eventually.TrueAsync(() => db.Queued.OfType<LoadCharacterWork>().Any());
            LoadCharacterWork load = db.Take<LoadCharacterWork>();
            owner.Post(new CreateCharacter(Account, _session, Create("Cccc Dddd")));
            load.Character = Record(5, 1);
            load.Complete(WorkResult.Committed);
            await Eventually.TrueAsync(() => map.Posted.Count == 1);

            var other = new FakeClientSession();
            await ListOnAsync(owner, db, other, Account, InWorld(5, "Rebaser Head", 1));
            owner.Post(new SelectCharacter(Account, other, 5));
            await SyncAsync(owner, other);

            // The one refill, thrown on the load's message, is what put the handler in its fault path.
            Assert.Equal(1, Volatile.Read(ref refills));
            Assert.Empty(db.Queued.OfType<LoadCharacterWork>());
            Assert.Empty(other.Sent.OfType<CharacterSelectFailed>());
            Assert.Empty(_session.Sent.OfType<CharacterSelectFailed>());
        }
        finally
        {
            await owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        }
    }

    [Fact]
    public async Task ACharacterLoadedFaultBeforeTheArrivalIsPostedFreesTheCharacterAndAnswersGeneral()
    {
        FakeMapInbox map = _maps.Add(1);
        map.Throwing = true;
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await SelectAndLoadAsync(_session, 5, Record(5, 1));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(SelectReasons.General, _session.Single<CharacterSelectFailed>().Unknown0);

        map.Throwing = false;
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
    }

    [Fact]
    public async Task ASelectFaultAfterItsPresenceWasCreatedFreesTheCharacterForAnotherSession()
    {
        _maps.Add(1);
        var other = new FakeClientSession();
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        _db.ThrowOn = work => work is LoadCharacterWork;
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.DisconnectReason is not null);

        _db.ThrowOn = null;
        await ListAsync(other, Account, InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, other, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterSelectFailed>());
    }

    [Fact]
    public async Task ANewerParkedSelectReplacesAnOlderOneWhichIsAnsweredGeneral()
    {
        FakeMapInbox map = _maps.Add(1);
        var older = new FakeClientSession();
        var newer = new FakeClientSession();
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await EnterAsync(_session, 5, map);
        await ListAsync(older, Account, InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, older, 5));
        await ListAsync(newer, Account, InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, newer, 5));
        await Eventually.TrueAsync(() => older.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(SelectReasons.General, older.Single<CharacterSelectFailed>().Unknown0);

        _owner.Post(new CharacterReturned(Account, 5, new CharacterLocation(1, 0, new Vector3(1, 2, 3), 0)));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        load.Character = Record(5, 1);
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => map.Posted.Count == 2);
        Assert.Same(newer, Assert.IsType<CharacterArrived>(map.Posted[1]).Session);
        Assert.Empty(newer.Sent.OfType<CharacterSelectFailed>());
    }

    [Fact]
    public async Task ClosingAParkedSessionDropsItsSelectSoTheReturnStartsNoLoadForIt()
    {
        FakeMapInbox map = _maps.Add(1);
        var waiting = new FakeClientSession();
        await ListAsync(InWorld(5, "Rebaser Head", 1));
        await EnterAsync(_session, 5, map);
        await ListAsync(waiting, Account, InWorld(5, "Rebaser Head", 1));
        _owner.Post(new SelectCharacter(Account, waiting, 5));
        _owner.Post(new RealmSessionClosed(Account, waiting));
        _owner.Post(new CharacterReturned(Account, 5, new CharacterLocation(1, 0, new Vector3(1, 2, 3), 0)));

        // The list's load queues behind the return, so once it is answered the return has been handled.
        await ListAsync(new FakeClientSession(), Account, InWorld(5, "Rebaser Head", 1));
        Assert.Empty(_db.Queued.OfType<LoadCharacterWork>());
    }

    [Fact]
    public async Task ASelectUsesTheLoadedRecordsWorldOverTheListsWorld()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync(InWorld(5, "Rebaser Head", 9));
        await SelectAndLoadAsync(_session, 5, Record(5, 1));
        await Eventually.TrueAsync(() => map.Posted.Count == 1);
        Assert.Equal((ushort)1, Assert.IsType<CharacterArrived>(map.Posted[0]).Character.Location.WorldId);
        Assert.Empty(_session.Sent.OfType<CharacterSelectFailed>());
    }

    [Fact]
    public async Task CreatesInFlightCountAgainstTheCap()
    {
        await ListAsync(Summary(5, "Aaaa Bbbb"));
        _owner.Post(new CreateCharacter(Account, _session, Create("Cccc Dddd")));
        _owner.Post(new CreateCharacter(Account, _session, Create("Eeee Ffff")));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.AccountFull, _session.Single<CharacterCreateResult>().Result);
        Assert.Single(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Fact]
    public async Task CreatesInFlightCountAgainstTheCapAcrossSessions()
    {
        var other = new FakeClientSession();
        await ListAsync(Summary(5, "Aaaa Bbbb"));
        _owner.Post(new CreateCharacter(Account, _session, Create("Cccc Dddd")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());

        // The new session's load cannot see the uncommitted create.
        await ListAsync(other, Account, Summary(5, "Aaaa Bbbb"));
        _owner.Post(new CreateCharacter(Account, other, Create("Eeee Ffff")));
        await Eventually.TrueAsync(() => other.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.AccountFull, other.Single<CharacterCreateResult>().Result);
        Assert.Single(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Theory]
    [InlineData(true, null, true)]
    [InlineData(false, RealmContext.LiveNameIndex, false)]
    [InlineData(false, "other_constraint", false)]
    public async Task APendingCreateReleasesItsPlaceOnEveryOutcome(bool committed, string? constraint, bool accountFull)
    {
        await ListAsync(Summary(5, "Aaaa Bbbb"));
        _owner.Post(new CreateCharacter(Account, _session, Create("Cccc Dddd")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        _db.Take<CreateCharacterWork>().Complete(committed ? WorkResult.Committed : WorkResult.RefusedBy(constraint));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());

        _owner.Post(new CreateCharacter(Account, _session, Create("Eeee Ffff")));
        if (accountFull)
        {
            await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Count() == 2);
            Assert.Equal(CreateResults.AccountFull, _session.Sent.OfType<CharacterCreateResult>().Last().Result);
        }
        else
        {
            await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        }
    }

    [Fact]
    public async Task ACreateRefusedByAnotherConstraintAnswersTheGeneralFailure()
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        _db.Take<CreateCharacterWork>().Complete(WorkResult.RefusedBy("other_constraint"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.General, _session.Single<CharacterCreateResult>().Result);
    }

    [Fact]
    public async Task APathPastTheLastAnswersTheGeneralFailure()
    {
        await ListAsync();
        CharacterCreate request = Create("Rebaser Head");
        request.Path = 4;
        _owner.Post(new CreateCharacter(Account, _session, request));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.General, _session.Single<CharacterCreateResult>().Result);
        Assert.Empty(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Fact]
    public async Task ACreateThatRepeatsALookIdAnswersTheGeneralFailure()
    {
        await ListAsync();
        CharacterCreate request = Create("Rebaser Head");
        request.LookIds.AddRange([4, 9, 4]);
        request.LookValues.AddRange([1, 2, 3]);
        _owner.Post(new CreateCharacter(Account, _session, request));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.General, _session.Single<CharacterCreateResult>().Result);
        Assert.Empty(_db.Queued.OfType<CreateCharacterWork>());

        // The refusal holds no name: the same name is free for the next create.
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
    }

    [Fact]
    public async Task ACreateWhoseLookIdsDifferIsWritten()
    {
        await ListAsync();
        CharacterCreate request = Create("Rebaser Head");
        request.LookIds.AddRange([4, 9]);
        request.LookValues.AddRange([1, 2]);
        _owner.Post(new CreateCharacter(Account, _session, request));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterCreateResult>());
    }

    [Fact]
    public async Task ADeleteThatDoesNotCommitAnswersTheGeneralFailureAndKeepsTheCharacter()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new DeleteCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<DeleteCharacterWork>().Any());
        _db.Take<DeleteCharacterWork>().Complete(WorkResult.RefusedBy("other_constraint"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterDeleteResult>().Any());
        Assert.Equal(DeleteResults.General, _session.Single<CharacterDeleteResult>().Result);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Single(_session.Sent.OfType<CharacterList>().Last().Characters);
    }

    [Fact]
    public async Task ARenameOfACharacterNotListedAnswersResult14()
    {
        await ListAsync();
        _owner.Post(new RenameCharacter(Account, _session, 99, "Other Name"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(RenameResults.InternalError, _session.Single<CharacterRenameResult>().Result);
    }

    [Fact]
    public async Task ARenameToANameOutsideTheRulesAnswersResult15()
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = true });
        _owner.Post(new RenameCharacter(Account, _session, 5, "Otherx"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(RenameResults.NameNotAllowed, _session.Single<CharacterRenameResult>().Result);
        Assert.Empty(_db.Queued.OfType<RenameCharacterWork>());
    }

    [Fact]
    public async Task ARenameToANameInTheListAnswersResult16()
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = true }, Summary(6, "Cccc Dddd"));
        _owner.Post(new RenameCharacter(Account, _session, 5, "cccc DDDD"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(RenameResults.NameTaken, _session.Single<CharacterRenameResult>().Result);
        Assert.Empty(_db.Queued.OfType<RenameCharacterWork>());
    }

    [Theory]
    [InlineData(RealmContext.LiveNameIndex, 16)]
    [InlineData("other_constraint", 14)]
    public async Task ARenameTheDatabaseRefusesAnswersItsResult(string constraint, int expected)
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = true });
        _owner.Post(new RenameCharacter(Account, _session, 5, "Other Name"));
        await Eventually.TrueAsync(() => _db.Queued.OfType<RenameCharacterWork>().Any());
        _db.Take<RenameCharacterWork>().Complete(WorkResult.RefusedBy(constraint));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(expected, _session.Single<CharacterRenameResult>().Result);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        CharacterList.Character kept = Assert.Single(_session.Sent.OfType<CharacterList>().Last().Characters);
        Assert.Equal("Rebaser Head", kept.Name);
        Assert.True(kept.Unknown17);
    }

    [Fact]
    public async Task ASecondRenameWhileOneIsInFlightAnswersResult17()
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = true });
        _owner.Post(new RenameCharacter(Account, _session, 5, "Other Name"));
        _owner.Post(new RenameCharacter(Account, _session, 5, "Third Name"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(RenameResults.NoRenameAvailable, _session.Single<CharacterRenameResult>().Result);
        Assert.Single(_db.Queued.OfType<RenameCharacterWork>());
    }

    [Fact]
    public async Task AClosedSessionDropsItsCharacters()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new RealmSessionClosed(Account, _session));
        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
    }

    [Fact]
    public async Task AnotherSessionsCloseKeepsTheCharacters()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new RealmSessionClosed(Account, new FakeClientSession()));
        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Empty(_db.Queued.OfType<LoadCharactersWork>());
    }

    [Fact]
    public async Task ASecondSessionReloadsAndTheFirstLosesItsState()
    {
        var other = new FakeClientSession();
        await ListAsync(Summary(5, "Rebaser Head"));
        await ListAsync(other, Account, Summary(6, "Cccc Dddd"));
        Assert.Equal("Cccc Dddd", Assert.Single(other.Single<CharacterList>().Characters).Name);

        _owner.Post(new CreateCharacter(Account, _session, Create("Eeee Ffff")));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.General, _session.Single<CharacterCreateResult>().Result);
    }

    [Fact]
    public async Task AnEmptyAllocatorAsksForOneBlockAndOnlyOne()
    {
        var db = new FakeWriteBehind();
        RealmOwner owner = StartOwner(db, null);
        try
        {
            await ListOnAsync(owner, db, _session, Account);
            owner.Post(new ListCharacters(Account, _session));
            await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
            Assert.Single(db.Queued.OfType<TakeIdBlockWork>());

            TakeIdBlockWork taken = db.Take<TakeIdBlockWork>();
            taken.Block = new IdBlock(7001, 1000);
            taken.Complete(WorkResult.Committed);
            owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
            await Eventually.TrueAsync(() => db.Queued.OfType<CreateCharacterWork>().Any());
            db.Take<CreateCharacterWork>().Complete(WorkResult.Committed);
            await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
            Assert.Equal(7001UL, _session.Single<CharacterCreateResult>().CharacterId);
            Assert.Empty(db.Queued.OfType<TakeIdBlockWork>());
        }
        finally
        {
            await owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        }
    }

    [Fact]
    public async Task TheRefillIsAskedForBelowHalfABlockAndOnlyOnce()
    {
        var db = new FakeWriteBehind();
        RealmOwner owner = StartOwner(db, 501);
        try
        {
            await ListOnAsync(owner, db, _session, Account);

            // 501 held, 500 after this create: not yet below the mark of 500.
            owner.Post(new CreateCharacter(Account, _session, Create("Aaaa Bbbb")));
            await Eventually.TrueAsync(() => db.Queued.OfType<CreateCharacterWork>().Any());
            await SyncAsync(owner, _session);
            Assert.Empty(db.Queued.OfType<TakeIdBlockWork>());

            owner.Post(new CreateCharacter(Account, _session, Create("Cccc Dddd")));
            await Eventually.TrueAsync(() => db.Queued.OfType<TakeIdBlockWork>().Any());

            owner.Post(new CreateCharacter(Account, _session, Create("Eeee Ffff")));
            await Eventually.TrueAsync(() => db.Queued.OfType<CreateCharacterWork>().Count() == 3);
            await SyncAsync(owner, _session);
            Assert.Single(db.Queued.OfType<TakeIdBlockWork>());
        }
        finally
        {
            await owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        }
    }

    private RealmOwner StartOwner(FakeWriteBehind db, int? ids)
    {
        var owner = new RealmOwner(db, s_catalog, s_filter, _maps, _time, Options.Create(new RealmOptions()), NullLogger<RealmOwner>.Instance);
        if (ids is int count)
            owner.Post(new IdBlockTaken(new IdBlock(5001, count)));
        owner.Start();
        return owner;
    }

    // A cached list answer is sent only after every earlier message has been handled.
    private static async Task SyncAsync(RealmOwner owner, FakeClientSession session)
    {
        int before = session.Sent.OfType<CharacterList>().Count();
        owner.Post(new ListCharacters(Account, session));
        await Eventually.TrueAsync(() => session.Sent.OfType<CharacterList>().Count() == before + 1);
    }

    private Task ListAsync(params CharacterSummary[] characters) => ListAsync(_session, Account, characters);

    private Task ListAsync(FakeClientSession session, long accountId, params CharacterSummary[] characters) =>
        ListOnAsync(_owner, _db, session, accountId, characters);

    private static async Task ListOnAsync(
        RealmOwner owner, FakeWriteBehind db, FakeClientSession session, long accountId, params CharacterSummary[] characters)
    {
        int before = session.Sent.OfType<CharacterList>().Count();
        owner.Post(new ListCharacters(accountId, session));
        await Eventually.TrueAsync(() => db.Queued.OfType<LoadCharactersWork>().Any(w => w.Keys[0] == PersistKey.Account(accountId)));
        LoadCharactersWork load = db.Queued.OfType<LoadCharactersWork>().First(w => w.Keys[0] == PersistKey.Account(accountId));
        db.Remove(load);
        load.Characters = [.. characters];
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => session.Sent.OfType<CharacterList>().Count() > before);
    }

    private static CharacterSummary Summary(ulong id, string name) =>
        new(id, Account, name, Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, WorldId: 0, ZoneId: 0, Position: default,
            Disabled: false, RequiresRename: false, Bones: []);

    private static CharacterSummary InWorld(ulong id, string name, ushort world) => Summary(id, name) with { WorldId = world };

    private static CharacterRecord Record(ulong id, ushort world) =>
        new(id, Account, "Rebaser Head", Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, TotalXp: 0,
            new CharacterLocation(world, 0, new Vector3(1, 2, 3), 0));

    // Selects, takes the queued load and completes it with the record.
    private async Task SelectAndLoadAsync(FakeClientSession session, ulong id, CharacterRecord? record)
    {
        _owner.Post(new SelectCharacter(Account, session, id));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        load.Character = record;
        load.Complete(WorkResult.Committed);
    }

    // Selects, loads the character in the map's world and waits for the map's next post.
    private async Task EnterAsync(FakeClientSession session, ulong id, FakeMapInbox map)
    {
        int before = map.Posted.Count;
        await SelectAndLoadAsync(session, id, Record(id, map.WorldId));
        await Eventually.TrueAsync(() => map.Posted.Count > before);
    }

    private static CharacterCreate Create(string name) => new() { CreationId = 1, Name = name, Path = 1 };
}
