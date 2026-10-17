using System.Collections.Immutable;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

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
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1002, 8, "REBASER HEAD"), result => second.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await first.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
        WorkResult refused = await second.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(WorkOutcome.Refused, refused.Outcome);
        Assert.Equal(RealmContext.LiveNameIndex, refused.Constraint);
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
