using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Persistence;
using NexusTomorrow.Testing;
using Npgsql;

namespace NexusTomorrow.Persistence.Tests;

public sealed class WriteBehindQueueTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(10);

    private static readonly PersistenceContexts s_unused = PersistenceContexts.Create("Host=unused");

    private static WriteBehindQueue Queue(FakeTimeProvider time, int workers = 4) =>
        new(s_unused, time, Options.Create(new PersistenceOptions { ConnectionString = "unused", SaveWorkers = workers }), NullLogger<WriteBehindQueue>.Instance);

    private static Task Within(Task task) => task.WaitAsync(s_bound);

    [Fact]
    public async Task WorkOnOneKeyRunsInArrivalOrderOneAtATime()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        var log = new RunLog();
        GatedWork a = new(log, "a", PersistKey.Account(1));
        GatedWork b = new(log, "b", PersistKey.Account(1));
        queue.Enqueue(a);
        queue.Enqueue(b);

        await Within(a.Started);
        Assert.False(b.Started.IsCompleted);
        a.Finish();
        await Within(b.Started);
        b.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
        Assert.Equal(["a", "b"], log.Order);
    }

    [Fact]
    public async Task WorkOnDifferentKeysRunsAtTheSameTime()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        var log = new RunLog();
        GatedWork a = new(log, "a", PersistKey.Account(1));
        GatedWork b = new(log, "b", PersistKey.Account(2));
        queue.Enqueue(a);
        queue.Enqueue(b);
        await Within(Task.WhenAll(a.Started, b.Started));
        a.Finish();
        b.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AWorkOnTwoKeysWaitsForBothAndKeepsTheGlobalOrder()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        var log = new RunLog();
        GatedWork a = new(log, "a", PersistKey.Account(1));
        GatedWork b = new(log, "b", PersistKey.Account(1), PersistKey.Character(9));
        GatedWork c = new(log, "c", PersistKey.Character(9));
        queue.Enqueue(a);
        queue.Enqueue(b);
        queue.Enqueue(c);

        await Within(a.Started);
        Assert.False(b.Started.IsCompleted);
        Assert.False(c.Started.IsCompleted); // c arrived after b, which holds the head of the character:9 queue
        a.Finish();
        await Within(b.Started);
        Assert.False(c.Started.IsCompleted);
        b.Finish();
        await Within(c.Started);
        c.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
        Assert.Equal(["a", "b", "c"], log.Order);
    }

    [Fact]
    public async Task ASnapshotMergesIntoTheWaitingOneNeverTheRunningOne()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        var log = new RunLog();
        GatedWork running = new(log, "running", PersistKey.Character(1)) { Mergeable = true };
        GatedWork waiting = new(log, "waiting", PersistKey.Character(1)) { Mergeable = true };
        GatedWork newer = new(log, "newer", PersistKey.Character(1)) { Mergeable = true };
        queue.Enqueue(running);
        await Within(running.Started);
        queue.Enqueue(waiting);
        queue.Enqueue(newer);

        Assert.Equal(["newer"], waiting.Merged);
        Assert.Empty(running.Merged);
        running.Finish();
        await Within(waiting.Started);
        waiting.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
        Assert.Equal(["running", "waiting"], log.Order);
        Assert.Equal(0, queue.Backlog);
    }

    [Fact]
    public async Task AWorkOnTwoKeysNeverMerges()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        var log = new RunLog();
        GatedWork running = new(log, "running", PersistKey.Character(1));
        GatedWork waiting = new(log, "waiting", PersistKey.Character(1)) { Mergeable = true };
        GatedWork pair = new(log, "pair", PersistKey.Character(1), PersistKey.Character(2)) { Mergeable = true };
        queue.Enqueue(running);
        await Within(running.Started);
        queue.Enqueue(waiting);
        queue.Enqueue(pair);
        Assert.Empty(waiting.Merged);
        running.Finish();
        await Within(waiting.Started);
        waiting.Finish();
        await Within(pair.Started);
        pair.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
        Assert.Equal(["running", "waiting", "pair"], log.Order);
    }

    [Fact]
    public async Task AFailedWorkIsRetriedWithBackoffAndKeepsItsPlace()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        var log = new RunLog();
        GatedWork flaky = new(log, "flaky", PersistKey.Account(1)) { FailuresLeft = 2 };
        GatedWork after = new(log, "after", PersistKey.Account(1));
        queue.Enqueue(flaky);
        queue.Enqueue(after);
        flaky.Finish();

        await Eventually.TrueAsync(() => !queue.IsAvailable);
        Assert.False(after.Started.IsCompleted);

        // The retry waits on the fake clock; step it until the third attempt succeeds and the next work starts.
        await Eventually.TrueAsync(() =>
        {
            time.Advance(TimeSpan.FromMilliseconds(50));
            return after.Started.IsCompleted;
        });

        after.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
        Assert.True(queue.IsAvailable);
        Assert.Equal(WorkOutcome.Committed, flaky.Result?.Outcome);
        Assert.Equal(["flaky", "flaky", "flaky", "after"], log.Order);
    }

    [Fact]
    public async Task AUniqueViolationCompletesTheWorkAsRefused()
    {
        var time = new FakeTimeProvider();
        await using WriteBehindQueue queue = Queue(time);
        queue.Start();
        GatedWork work = new(new RunLog(), "dup", PersistKey.Named("x")) { Throw = new PostgresException("duplicate", "ERROR", "ERROR", PostgresErrorCodes.UniqueViolation) };
        queue.Enqueue(work);
        work.Finish();
        await Within(queue.DrainAsync(TestContext.Current.CancellationToken));
        Assert.Equal(WorkOutcome.Refused, work.Result?.Outcome);
        Assert.True(queue.IsAvailable);
    }

    private sealed class RunLog
    {
        private readonly Lock _gate = new();
        private readonly List<string> _order = [];

        public List<string> Order
        {
            get
            {
                lock (_gate)
                    return [.. _order];
            }
        }

        public void Add(string name)
        {
            lock (_gate)
                _order.Add(name);
        }
    }

    private sealed class GatedWork : PersistenceWork
    {
        private readonly RunLog _log;
        private readonly string _name;
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _finish = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public GatedWork(RunLog log, string name, params PersistKey[] keys)
            : base(keys)
        {
            _log = log;
            _name = name;
        }

        public bool Mergeable { get; init; }

        public int FailuresLeft { get; set; }

        public Exception? Throw { get; init; }

        public List<string> Merged { get; } = [];

        public Task Started => _started.Task;

        public WorkResult? Result { get; private set; }

        public void Finish() => _finish.TrySetResult();

        public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
        {
            _log.Add(_name);
            _started.TrySetResult();
            await _finish.Task;
            if (Throw is not null)
                throw Throw;
            if (FailuresLeft > 0)
            {
                FailuresLeft--;
                throw new NpgsqlException("connection refused");
            }
        }

        public override bool TryMerge(PersistenceWork newer)
        {
            if (!Mergeable || newer is not GatedWork { Mergeable: true } other)
                return false;
            Merged.Add(other._name);
            return true;
        }

        public override void Complete(WorkResult result) => Result = result;
    }
}
