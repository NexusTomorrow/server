using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;
using Npgsql;

namespace NexusTomorrow.Persistence.Tests;

public sealed class CharacterWorkTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task ACreatedCharacterLoadsBackWithItsBones()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var done = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1001, 7, "Rebaser Head"), result => done.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await done.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        var loaded = new TaskCompletionSource<ImmutableArray<CharacterSummary>>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new LoadCharactersWork(7, characters => loaded.SetResult(characters)));
        CharacterSummary character = Assert.Single(await loaded.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken));
        Assert.Equal(1001UL, character.Id);
        Assert.Equal("Rebaser Head", character.Name);
        Assert.Equal([0.25f, -0.5f], character.Bones);
    }

    [Fact]
    public async Task ATakenNameIsRefusedByTheLiveNameIndex()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var first = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var second = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1001, 7, "Rebaser Head"), result => first.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await first.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        // The second create goes in only after the first committed: the two share no key, so queued together they could commit in either order.
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1002, 8, "REBASER HEAD"), result => second.SetResult(result)));
        WorkResult refused = await second.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(WorkOutcome.Refused, refused.Outcome);
        Assert.Equal(RealmContext.LiveNameIndex, refused.Constraint);
    }

    // Two customization rows under one label share a key (RealmContext's character_customization). The work is applied directly,
    // never queued: the queue retries anything but a unique violation, so this work would hold its keys forever there, which is why
    // RealmOwner refuses a repeated look id.
    [Fact]
    public async Task TwoCustomizationsUnderOneLabelFailEveryAttemptWithoutAUniqueViolation()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        NewCharacter character = Databases.NewCharacter(1001, 7, "Rebaser Head") with
        {
            Customizations = [(Label: 1u, Value: 2u), (Label: 1u, Value: 3u)],
        };
        var work = new CreateCharacterWork(character, _ => { });
        for (int attempt = 0; attempt < 2; attempt++)
        {
            InvalidOperationException thrown = await Assert.ThrowsAsync<InvalidOperationException>(
                () => work.ApplyAsync(database.Contexts, TestContext.Current.CancellationToken));
            for (Exception? cause = thrown; cause is not null; cause = cause.InnerException)
                Assert.False(cause is PostgresException);
        }

        await using RealmContext context = database.Contexts.CreateRealm();
        Assert.False(await context.Characters.AnyAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ADeletedCharacterNoLongerLoads()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var created = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1001, 7, "Rebaser Head"), result => created.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await created.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        var deleted = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new DeleteCharacterWork(7, 1001, DateTimeOffset.UnixEpoch, result => deleted.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await deleted.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        var loaded = new TaskCompletionSource<ImmutableArray<CharacterSummary>>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new LoadCharactersWork(7, characters => loaded.SetResult(characters)));
        Assert.Empty(await loaded.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ARenameToATakenNameIsRefused()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        foreach ((ulong id, string name) in new[] { (1001UL, "Rebaser Head"), (1002UL, "Other Name") })
        {
            var created = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(id, 7, name), result => created.SetResult(result)));
            Assert.Equal(WorkOutcome.Committed, (await created.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
        }

        await using (RealmContext context = database.Contexts.CreateRealm())
        {
            await context.Characters.Where(c => c.Id == 1002).ExecuteUpdateAsync(
                set => set.SetProperty(c => c.RequiresRename, true), TestContext.Current.CancellationToken);
        }

        // Each rename goes in only after the one before it finished, so the order is fixed.
        var taken = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new RenameCharacterWork(7, 1002, "REBASER HEAD", result => taken.SetResult(result)));
        WorkResult refused = await taken.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(WorkOutcome.Refused, refused.Outcome);
        Assert.Equal(RealmContext.LiveNameIndex, refused.Constraint);

        var free = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new RenameCharacterWork(7, 1002, "Free Name", result => free.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await free.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        var loaded = new TaskCompletionSource<ImmutableArray<CharacterSummary>>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new LoadCharactersWork(7, characters => loaded.SetResult(characters)));
        CharacterSummary renamed = (await loaded.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Single(c => c.Id == 1002);
        Assert.Equal("Free Name", renamed.Name);
        Assert.False(renamed.RequiresRename);
    }

    [Fact]
    public async Task ASecondRenameOfTheSameMarkChangesNothing()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var created = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1001, 7, "Rebaser Head"), result => created.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await created.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        await using (RealmContext context = database.Contexts.CreateRealm())
        {
            await context.Characters.Where(c => c.Id == 1001).ExecuteUpdateAsync(
                set => set.SetProperty(c => c.RequiresRename, true), TestContext.Current.CancellationToken);
        }

        // Both renames share the character's key, so they run in this order; the mark is gone after the first.
        var first = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var second = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new RenameCharacterWork(7, 1001, "First Name", result => first.SetResult(result)));
        queue.Enqueue(new RenameCharacterWork(7, 1001, "Second Name", result => second.SetResult(result)));
        await first.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
        await second.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        var loaded = new TaskCompletionSource<ImmutableArray<CharacterSummary>>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new LoadCharactersWork(7, characters => loaded.SetResult(characters)));
        CharacterSummary character = Assert.Single(await loaded.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken));
        Assert.Equal("First Name", character.Name);
    }

    [Fact]
    public async Task AnIdBlockIsTheNextThousand()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var blocks = new List<IdBlock>();
        for (int i = 0; i < 2; i++)
        {
            var taken = new TaskCompletionSource<IdBlock>(TaskCreationOptions.RunContinuationsAsynchronously);
            queue.Enqueue(new TakeIdBlockWork(block => taken.SetResult(block)));
            blocks.Add(await taken.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken));
        }

        Assert.Equal([new IdBlock(1, 1000), new IdBlock(1001, 1000)], blocks);
    }
}

internal static partial class Databases
{
    public static WriteBehindQueue Queue(PostgresTestDatabase database)
    {
        var queue = new WriteBehindQueue(database.Contexts, TimeProvider.System,
            Options.Create(new PersistenceOptions { ConnectionString = database.ConnectionString }), NullLogger<WriteBehindQueue>.Instance);
        queue.Start();
        return queue;
    }

    public static NewCharacter NewCharacter(ulong id, long accountId, string name) => new(
        id, accountId, name, CreationId: 1, Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, TotalXp: 0,
        WorldId: 0, ZoneId: 0, Position: default, CreatedAt: DateTimeOffset.UnixEpoch,
        Customizations: [(Label: 1u, Value: 2u)], Bones: [0.25f, -0.5f]);
}
