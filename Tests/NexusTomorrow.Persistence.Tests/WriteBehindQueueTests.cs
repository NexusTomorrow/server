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

    private static Task Within(Task task) => task.WaitAsync(s_bound);

    [Fact]
    public async Task WorkOnOneKeyRunsInArrivalOrderOneAtATime()
    {
        await using var harness = new Harness();
        GatedWork a = new(harness.Log, "a", PersistKey.Account(1));
        GatedWork b = new(harness.Log, "b", PersistKey.Account(1));
        harness.Enqueue(a);
        harness.Enqueue(b);

        await Within(a.Started);
        Assert.False(b.Started.IsCompleted);
        a.Finish();
        await Within(b.Started);
        b.Finish();
        await harness.DrainAsync();
        Assert.Equal(["a", "b"], harness.Log.Order);
    }

    [Fact]
    public async Task WorkOnDifferentKeysRunsAtTheSameTime()
    {
        await using var harness = new Harness();
        GatedWork a = new(harness.Log, "a", PersistKey.Account(1));
        GatedWork b = new(harness.Log, "b", PersistKey.Account(2));
        harness.Enqueue(a);
        harness.Enqueue(b);
        await Within(Task.WhenAll(a.Started, b.Started));
        a.Finish();
        b.Finish();
        await harness.DrainAsync();
    }

    [Fact]
    public async Task AWorkOnTwoKeysWaitsForBothAndKeepsTheGlobalOrder()
    {
        await using var harness = new Harness();
        GatedWork a = new(harness.Log, "a", PersistKey.Account(1));
        GatedWork b = new(harness.Log, "b", PersistKey.Account(1), PersistKey.Character(9));
        GatedWork c = new(harness.Log, "c", PersistKey.Character(9));
        harness.Enqueue(a);
        harness.Enqueue(b);
        harness.Enqueue(c);

        await Within(a.Started);
        Assert.False(b.Started.IsCompleted);
        Assert.False(c.Started.IsCompleted); // c arrived after b, which holds the head of the character:9 queue
        a.Finish();
        await Within(b.Started);
        Assert.False(c.Started.IsCompleted);
        b.Finish();
        await Within(c.Started);
        c.Finish();
        await harness.DrainAsync();
        Assert.Equal(["a", "b", "c"], harness.Log.Order);
    }

    [Fact]
    public async Task ASnapshotMergesIntoTheWaitingOneNeverTheRunningOneAndEveryMergedWorkIsCompleted()
    {
        await using var harness = new Harness();
        GatedWork running = new(harness.Log, "running", PersistKey.Character(1)) { Mergeable = true };
        GatedWork waiting = new(harness.Log, "waiting", PersistKey.Character(1)) { Mergeable = true };
        GatedWork newer = new(harness.Log, "newer", PersistKey.Character(1)) { Mergeable = true };
        GatedWork newest = new(harness.Log, "newest", PersistKey.Character(1)) { Mergeable = true };
        harness.Enqueue(running);
        await Within(running.Started);
        harness.Enqueue(waiting);
        harness.Enqueue(newer);
        harness.Enqueue(newest);

        Assert.Equal(["newer", "newest"], waiting.Merged.Select(work => work.Name));
        Assert.Empty(running.Merged);
        Assert.Equal(2, harness.Queue.Backlog);
        running.Finish();
        await Within(waiting.Started);
        waiting.Finish();
        await harness.DrainAsync();
        Assert.Equal(["running", "waiting"], harness.Log.Order);
        Assert.Equal(0, harness.Queue.Backlog);
        Assert.Equal(WorkOutcome.Committed, newer.Result?.Outcome);
        Assert.Equal(WorkOutcome.Committed, newest.Result?.Outcome);
    }

    [Fact]
    public async Task AWorkOnTwoKeysNeverMerges()
    {
        await using var harness = new Harness();
        GatedWork running = new(harness.Log, "running", PersistKey.Character(1));
        GatedWork waiting = new(harness.Log, "waiting", PersistKey.Character(1)) { Mergeable = true };
        GatedWork pair = new(harness.Log, "pair", PersistKey.Character(1), PersistKey.Character(2)) { Mergeable = true };
        harness.Enqueue(running);
        await Within(running.Started);
        harness.Enqueue(waiting);
        harness.Enqueue(pair);
        Assert.Empty(waiting.Merged);
        running.Finish();
        await Within(waiting.Started);
        waiting.Finish();
        await Within(pair.Started);
        pair.Finish();
        await harness.DrainAsync();
        Assert.Equal(["running", "waiting", "pair"], harness.Log.Order);
    }

    [Fact]
    public async Task AFailedWorkIsRetriedAndKeepsItsPlaceWhileTheDatabaseIsDown()
    {
        await using var harness = new Harness();
        GatedWork flaky = new(harness.Log, "flaky", PersistKey.Account(1)) { Failures = { new NpgsqlException("connection refused"), new NpgsqlException("connection refused") } };
        GatedWork after = new(harness.Log, "after", PersistKey.Account(1));
        harness.Enqueue(flaky);
        harness.Enqueue(after);
        flaky.Finish();

        await Eventually.TrueAsync(() => !harness.Queue.IsAvailable);
        Assert.False(after.Started.IsCompleted);
        await harness.StepRetriesAsync(flaky, retries: 2);

        await Within(after.Started);
        after.Finish();
        await harness.DrainAsync();
        Assert.True(harness.Queue.IsAvailable);
        Assert.Equal(WorkOutcome.Committed, flaky.Result?.Outcome);
        Assert.Equal(["flaky", "flaky", "flaky", "after"], harness.Log.Order);
    }

    [Fact]
    public async Task RetriesWaitTheFirstDelayThenDoubleIt()
    {
        await using var harness = new Harness(first: TimeSpan.FromMilliseconds(100), max: TimeSpan.FromSeconds(30));
        GatedWork flaky = new(harness.Log, "flaky", PersistKey.Account(1)) { Failures = { new NpgsqlException("down"), new NpgsqlException("down"), new NpgsqlException("down") } };
        harness.Enqueue(flaky);
        flaky.Finish();

        TimeSpan[] delays = await harness.StepRetriesAsync(flaky, retries: 3);
        await harness.DrainAsync();
        TimeSpan[] expected = [TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(400)];
        Assert.Equal(expected, delays);
        Assert.Equal(WorkOutcome.Committed, flaky.Result?.Outcome);
    }

    [Fact]
    public async Task RetryDelaysStopDoublingAtTheMaximum()
    {
        await using var harness = new Harness(first: TimeSpan.FromMilliseconds(100), max: TimeSpan.FromMilliseconds(250));
        GatedWork flaky = new(harness.Log, "flaky", PersistKey.Account(1)) { Failures = { new NpgsqlException("down"), new NpgsqlException("down"), new NpgsqlException("down"), new NpgsqlException("down") } };
        harness.Enqueue(flaky);
        flaky.Finish();

        TimeSpan[] delays = await harness.StepRetriesAsync(flaky, retries: 4);
        await harness.DrainAsync();
        TimeSpan[] expected = [TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250)];
        Assert.Equal(expected, delays);
    }

    [Fact]
    public async Task AServerErrorThatIsNotAUniqueViolationIsRetriedNotRefused()
    {
        await using var harness = new Harness();
        var rejected = new PostgresException("could not serialize access", "ERROR", "ERROR", PostgresErrorCodes.SerializationFailure);
        GatedWork work = new(harness.Log, "serialized", PersistKey.Account(1)) { Failures = { rejected } };
        harness.Enqueue(work);
        work.Finish();

        await Eventually.TrueAsync(() => harness.Time.TimersCreated == 1);
        Assert.True(harness.Queue.IsAvailable); // the server answered, so it is reachable
        Assert.Null(work.Result);
        await harness.StepRetriesAsync(work, retries: 1);
        await harness.DrainAsync();
        Assert.Equal(WorkOutcome.Committed, work.Result?.Outcome);
        Assert.Equal(2, work.Attempts);
    }

    [Fact]
    public async Task AUniqueViolationCompletesTheWorkAsRefusedWithoutRetrying()
    {
        await using var harness = new Harness();
        GatedWork work = new(harness.Log, "dup", PersistKey.Named("x")) { Failures = { new PostgresException("duplicate", "ERROR", "ERROR", PostgresErrorCodes.UniqueViolation) } };
        harness.Enqueue(work);
        work.Finish();
        await harness.DrainAsync();
        Assert.Equal(WorkOutcome.Refused, work.Result?.Outcome);
        Assert.Equal(1, work.Attempts);
        Assert.Equal(0, harness.Time.TimersCreated);
        Assert.True(harness.Queue.IsAvailable);
    }

    [Fact]
    public async Task ACompletionThatThrowsStillReleasesTheKeyForTheNextWork()
    {
        await using var harness = new Harness();
        GatedWork bad = new(harness.Log, "bad", PersistKey.Account(1)) { ThrowOnComplete = true };
        GatedWork next = new(harness.Log, "next", PersistKey.Account(1));
        harness.Enqueue(bad);
        harness.Enqueue(next);
        bad.Finish();

        await Within(next.Started);
        next.Finish();
        await harness.DrainAsync();
        Assert.Equal(WorkOutcome.Committed, bad.Result?.Outcome);
        Assert.Equal(WorkOutcome.Committed, next.Result?.Outcome);
        Assert.Equal(0, harness.Queue.Backlog);
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

    // A fake clock that also records the timers the queue creates: the delay each was created with, and how many have fired. A timer counts as created once it is armed.
    private sealed class RecordingTimeProvider : FakeTimeProvider
    {
        private readonly Lock _gate = new();
        private readonly List<TimeSpan> _delays = [];
        private int _fired;

        public int TimersCreated
        {
            get
            {
                lock (_gate)
                    return _delays.Count;
            }
        }

        public int TimersFired => Volatile.Read(ref _fired);

        public TimeSpan DelayOf(int timer)
        {
            lock (_gate)
                return _delays[timer];
        }

        // A timer is counted only after the base provider has armed it with its due time, so a test that sees the count may advance
        // the clock knowing the timer will fire.
        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            ITimer timer = base.CreateTimer(
                timerState =>
                {
                    Interlocked.Increment(ref _fired);
                    callback(timerState);
                },
                state,
                dueTime,
                period);
            lock (_gate)
                _delays.Add(dueTime);
            return timer;
        }
    }

    // One test's queue and clock. Disposing releases every work still gated and the clock, so a test that fails part-way ends with
    // its queue drained instead of hanging the run.
    private sealed class Harness : IAsyncDisposable
    {
        private readonly List<GatedWork> _enqueued = [];
        private readonly TimeSpan _maxRetryDelay;

        public Harness(TimeSpan? first = null, TimeSpan? max = null)
        {
            _maxRetryDelay = max ?? TimeSpan.FromSeconds(30);
            var options = new PersistenceOptions
            {
                ConnectionString = "unused",
                FirstRetryDelay = first ?? TimeSpan.FromMilliseconds(100),
                MaxRetryDelay = _maxRetryDelay,
            };
            Queue = new WriteBehindQueue(s_unused, Time, Options.Create(options), NullLogger<WriteBehindQueue>.Instance);
            Queue.Start();
        }

        public RecordingTimeProvider Time { get; } = new();

        public RunLog Log { get; } = new();

        public WriteBehindQueue Queue { get; }

        public void Enqueue(GatedWork work)
        {
            _enqueued.Add(work);
            Queue.Enqueue(work);
        }

        public Task DrainAsync() => Within(Queue.DrainAsync(TestContext.Current.CancellationToken));

        // For each of the work's failed attempts in turn: wait for the queue to start its retry delay, check the retry stays
        // asleep one millisecond short of the delay and is released at it. Returns the delays the queue chose.
        public async Task<TimeSpan[]> StepRetriesAsync(GatedWork work, int retries)
        {
            var delays = new List<TimeSpan>();
            for (int retry = 1; retry <= retries; retry++)
            {
                await Eventually.TrueAsync(() => Time.TimersCreated == retry);
                TimeSpan delay = Time.DelayOf(retry - 1);
                delays.Add(delay);

                Time.Advance(delay - TimeSpan.FromMilliseconds(1));
                Assert.Equal(retry - 1, Time.TimersFired);
                Assert.Equal(retry, work.Attempts);

                Time.Advance(TimeSpan.FromMilliseconds(1));
                Assert.Equal(retry, Time.TimersFired);
            }

            return [.. delays];
        }

        public async ValueTask DisposeAsync()
        {
            foreach (GatedWork work in _enqueued)
                work.Finish();

            await Eventually.TrueAsync(() =>
            {
                if (Queue.Backlog > 0)
                    Time.Advance(_maxRetryDelay);
                return Queue.Backlog == 0;
            });
            await Queue.DisposeAsync();
        }
    }

    private sealed class GatedWork : PersistenceWork
    {
        private readonly RunLog _log;
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _finish = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _attempts;
        private int _failed;

        public GatedWork(RunLog log, string name, params PersistKey[] keys)
            : base(keys)
        {
            _log = log;
            Name = name;
        }

        public string Name { get; }

        public bool Mergeable { get; init; }

        public bool ThrowOnComplete { get; init; }

        // Thrown by the work's attempts in order, one per attempt, after its gate opens; later attempts succeed.
        public List<Exception> Failures { get; } = [];

        // The works folded into this one, which this one completes on their behalf.
        public List<GatedWork> Merged { get; } = [];

        public Task Started => _started.Task;

        public int Attempts => Volatile.Read(ref _attempts);

        public WorkResult? Result { get; private set; }

        public void Finish() => _finish.TrySetResult();

        public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _attempts);
            _log.Add(Name);
            _started.TrySetResult();
            await _finish.Task;
            if (_failed < Failures.Count)
                throw Failures[_failed++];
        }

        public override bool TryMerge(PersistenceWork newer)
        {
            if (!Mergeable || newer is not GatedWork { Mergeable: true } other)
                return false;
            Merged.Add(other);
            return true;
        }

        public override void Complete(WorkResult result)
        {
            Result = result;
            foreach (GatedWork merged in Merged)
                merged.Complete(result);
            if (ThrowOnComplete)
                throw new InvalidOperationException("The owner's completion failed.");
        }
    }
}
