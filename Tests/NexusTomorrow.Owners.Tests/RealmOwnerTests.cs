using System.Collections.Immutable;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Realm;
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

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly FakeWriteBehind _db = new();
    private readonly FakeClientSession _session = new();
    private readonly RealmOwner _owner;

    public RealmOwnerTests()
    {
        _owner = new RealmOwner(_db, s_catalog, _time, Options.Create(new RealmOptions { MaxCharactersPerAccount = 2 }), NullLogger<RealmOwner>.Instance);
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
    [InlineData(false, false, 2)]
    [InlineData(true, false, 1)]
    [InlineData(false, true, 3)]
    public async Task ASelectionAnswersSelectFailedWithItsReason(bool requiresRename, bool disabled, int reason)
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = requiresRename, Disabled = disabled });
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(reason, _session.Single<CharacterSelectFailed>().Reason);
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
        Assert.True(kept.RequiresRename);
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
        var owner = new RealmOwner(db, s_catalog, _time, Options.Create(new RealmOptions()), NullLogger<RealmOwner>.Instance);
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

    private static CharacterCreate Create(string name) => new() { CreationId = 1, Name = name, Path = 1 };
}
