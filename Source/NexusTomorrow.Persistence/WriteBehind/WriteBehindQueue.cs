using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace NexusTomorrow.Persistence;

/// <summary>
/// The write-behind queue (decisions.md D9 2c). Each key has a FIFO of work in one global arrival order; a work is dispatched once it
/// heads the FIFO of every key it names, so work on shared keys stays ordered and two works can never wait on each other. One work per
/// key runs at a time. A newer single-key work merges into the waiting one on its key when that one accepts. Failed work is retried
/// with backoff in place, never dropped; a unique violation completes it as refused.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "The write-behind queue is the design's own term; the type is not a collection.")]
public sealed class WriteBehindQueue : IWriteBehind, IAsyncDisposable
{
    private readonly PersistenceContexts _contexts;
    private readonly TimeProvider _time;
    private readonly PersistenceOptions _options;
    private readonly ILogger<WriteBehindQueue> _logger;
    private readonly Lock _gate = new();
    private readonly Dictionary<PersistKey, LinkedList<Entry>> _queues = [];
    private readonly Channel<Entry> _ready = Channel.CreateUnbounded<Entry>(new UnboundedChannelOptions { SingleWriter = false });
    private readonly CancellationTokenSource _stop = new();
    private readonly List<Task> _workers = [];
    private TaskCompletionSource? _drained;
    private int _backlog;
    private int _available = 1;

    /// <summary>
    /// Create a queue applying work with <paramref name="contexts"/>.
    /// </summary>
    public WriteBehindQueue(PersistenceContexts contexts, TimeProvider time, IOptions<PersistenceOptions> options, ILogger<WriteBehindQueue> logger)
    {
        _contexts = contexts;
        _time = time;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc/>
    public int Backlog => Volatile.Read(ref _backlog);

    /// <inheritdoc/>
    public bool IsAvailable => Volatile.Read(ref _available) == 1;

    /// <summary>
    /// Start the workers.
    /// </summary>
    public void Start()
    {
        for (int i = 0; i < _options.SaveWorkers; i++)
            _workers.Add(Task.Run(WorkAsync));
    }

    /// <inheritdoc/>
    public void Enqueue(PersistenceWork work)
    {
        lock (_gate)
        {
            if (work.Keys.Length == 1
                && _queues.TryGetValue(work.Keys[0], out LinkedList<Entry>? single)
                && single.Last?.Value is { Dispatched: false } waiting
                && waiting.Work.Keys.Length == 1
                && waiting.Work.TryMerge(work))
            {
                return;
            }

            var entry = new Entry(work);
            foreach (PersistKey key in work.Keys)
            {
                if (!_queues.TryGetValue(key, out LinkedList<Entry>? queue))
                    _queues[key] = queue = new LinkedList<Entry>();
                queue.AddLast(entry);
            }

            _backlog++;
            TryDispatch(entry);
        }
    }

    /// <summary>
    /// Complete when no work is queued or running. Work enqueued meanwhile is waited for too.
    /// </summary>
    public async Task DrainAsync(CancellationToken cancellationToken)
    {
        Task drained;
        lock (_gate)
        {
            if (_backlog == 0)
                return;
            _drained ??= new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            drained = _drained.Task;
        }

        PersistenceLog.Draining(_logger, Backlog);
        await drained.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Drain, then stop the workers.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DrainAsync(CancellationToken.None);
        _ready.Writer.TryComplete();
        await _stop.CancelAsync();
        await Task.WhenAll(_workers);
        _stop.Dispose();
    }

    // Under _gate: dispatch when the entry heads every one of its keys' queues.
    private void TryDispatch(Entry entry)
    {
        if (entry.Dispatched)
            return;
        foreach (PersistKey key in entry.Work.Keys)
        {
            if (_queues[key].First!.Value != entry)
                return;
        }

        entry.Dispatched = true;
        _ready.Writer.TryWrite(entry);
    }

    private void Finish(Entry entry)
    {
        lock (_gate)
        {
            foreach (PersistKey key in entry.Work.Keys)
            {
                LinkedList<Entry> queue = _queues[key];
                queue.RemoveFirst();
                if (queue.First is null)
                    _queues.Remove(key);
                else
                    TryDispatch(queue.First.Value);
            }

            _backlog--;
            if (_backlog == 0 && _drained is not null)
            {
                _drained.TrySetResult();
                _drained = null;
            }
        }
    }

    private async Task WorkAsync()
    {
        try
        {
            await foreach (Entry entry in _ready.Reader.ReadAllAsync(_stop.Token))
            {
                WorkResult result = await ApplyUntilDoneAsync(entry.Work);
                try
                {
                    entry.Work.Complete(result);
                }
                catch (Exception exception)
                {
                    // A completion posts to an owner; a bug there must not stop this worker or leave the keys held.
                    PersistenceLog.CompletionFailed(_logger, entry.Work.GetType().Name, exception);
                }

                Finish(entry);
            }
        }
        catch (OperationCanceledException) when (_stop.IsCancellationRequested)
        {
        }
    }

    private async Task<WorkResult> ApplyUntilDoneAsync(PersistenceWork work)
    {
        TimeSpan delay = _options.FirstRetryDelay;
        while (true)
        {
            try
            {
                await work.ApplyAsync(_contexts, _stop.Token);
                Volatile.Write(ref _available, 1);
                return WorkResult.Committed;
            }
            catch (Exception exception) when (Find<PostgresException>(exception) is { SqlState: PostgresErrorCodes.UniqueViolation } violation)
            {
                Volatile.Write(ref _available, 1);
                return WorkResult.RefusedBy(violation.ConstraintName);
            }
            catch (Exception exception) when (!_stop.IsCancellationRequested)
            {
                // A PostgresException means the server answered; any other Npgsql failure means it could not be reached.
                if (Find<NpgsqlException>(exception) is not null and not PostgresException)
                    Volatile.Write(ref _available, 0);
                PersistenceLog.Retrying(_logger, work.GetType().Name, delay, exception);
                await Task.Delay(delay, _time, _stop.Token);
                delay = TimeSpan.FromTicks(Math.Min(delay.Ticks * 2, _options.MaxRetryDelay.Ticks));
            }
        }
    }

    private static T? Find<T>(Exception? exception)
        where T : Exception
    {
        for (; exception is not null; exception = exception.InnerException)
        {
            if (exception is T found)
                return found;
        }

        return null;
    }

    private sealed class Entry
    {
        public Entry(PersistenceWork work)
        {
            Work = work;
        }

        public PersistenceWork Work { get; }

        public bool Dispatched { get; set; }
    }
}
