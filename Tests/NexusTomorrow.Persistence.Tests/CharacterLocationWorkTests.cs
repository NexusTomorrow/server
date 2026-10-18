using System.Numerics;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Persistence.Tests;

public sealed class CharacterLocationWorkTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    private static async Task CreateAsync(WriteBehindQueue queue)
    {
        var created = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new CreateCharacterWork(Databases.NewCharacter(1001, 7, "Rebaser Head"), result => created.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await created.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
    }

    private static Task<CharacterRecord?> LoadAsync(WriteBehindQueue queue, ulong characterId, long accountId)
    {
        var loaded = new TaskCompletionSource<CharacterRecord?>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new LoadCharacterWork(characterId, accountId, record => loaded.SetResult(record)));
        return loaded.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task ACreatedCharacterLoadsInFull()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        await CreateAsync(queue);

        CharacterRecord? record = await LoadAsync(queue, 1001, 7);

        Assert.NotNull(record);
        Assert.Equal("Rebaser Head", record.Name);
        Assert.Equal(7L, record.AccountId);
        Assert.Equal(0f, record.Location.Facing);
    }

    [Fact]
    public async Task ASavedLocationLoadsBack()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        await CreateAsync(queue);

        var location = new CharacterLocation(12, 34, new Vector3(1.5f, -2.25f, 3f), 0.75f);
        var saved = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new SaveCharacterLocationWork(1001, location, result => saved.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await saved.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        CharacterRecord? record = await LoadAsync(queue, 1001, 7);

        Assert.NotNull(record);
        Assert.Equal(location, record.Location);
    }

    [Fact]
    public async Task ALoadQueuedAfterASaveReadsTheSave()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        await CreateAsync(queue);

        var location = new CharacterLocation(12, 34, new Vector3(5f, 6f, 7f), 1f);
        queue.Enqueue(new SaveCharacterLocationWork(1001, location, _ => { }));
        CharacterRecord? record = await LoadAsync(queue, 1001, 7);

        Assert.NotNull(record);
        Assert.Equal(location, record.Location);
    }

    [Fact]
    public async Task AnotherAccountsOrADeletedCharacterLoadsAsNothing()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        await CreateAsync(queue);

        Assert.Null(await LoadAsync(queue, 1001, 8));

        var deleted = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new DeleteCharacterWork(7, 1001, DateTimeOffset.UnixEpoch, result => deleted.SetResult(result)));
        Assert.Equal(WorkOutcome.Committed, (await deleted.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        Assert.Null(await LoadAsync(queue, 1001, 7));
    }

    [Fact]
    public void ANewerSaveOfTheSameCharacterMergesAndKeepsBothCompletions()
    {
        int completions = 0;
        var older = new SaveCharacterLocationWork(5, new CharacterLocation(1, 1, Vector3.Zero, 0f), _ => completions++);
        var newer = new SaveCharacterLocationWork(5, new CharacterLocation(1, 2, Vector3.One, 1f), _ => completions++);

        bool merged = older.TryMerge(newer);
        older.Complete(WorkResult.Committed);

        Assert.True(merged);
        Assert.Equal(new CharacterLocation(1, 2, Vector3.One, 1f), older.Location);
        Assert.Equal(2, completions);
    }

    [Fact]
    public void ASaveDoesNotMergeAnotherCharactersSaveOrALoad()
    {
        var save = new SaveCharacterLocationWork(5, new CharacterLocation(1, 1, Vector3.Zero, 0f), _ => { });

        Assert.False(save.TryMerge(new SaveCharacterLocationWork(6, new CharacterLocation(1, 1, Vector3.Zero, 0f), _ => { })));
        Assert.False(save.TryMerge(new LoadCharacterWork(5, 7, _ => { })));
    }
}
