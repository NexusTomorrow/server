using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway;
using NexusTomorrow.Gateway.Kestrel;
using NexusTomorrow.Maps;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Server;

/// <summary>
/// Brings the core up before Kestrel listens and takes it down after Kestrel has stopped accepting.
/// <para>
/// Start: refuse a schema with pending migrations, start the write-behind queue, hand the realm owner its first character id block
/// (waiting at most <see cref="PersistenceOptions.FirstIdBlockTimeout"/>), start both owners, start the map scheduler, then give
/// Kestrel its three listeners. Kestrel binds only after this start, so nothing listens before the core is ready. A start that fails
/// after the queue began stops the scheduler and the owners and drops the queue's start-up work, because no player has saved
/// anything yet.
/// </para>
/// <para>
/// Stop: stop every map, which hands each character back to the realm with its save queued; stop the map threads; stop both owners
/// (each handles what it has queued); then wait for every queued save to commit. Nothing bounds that wait, because a bound would
/// drop saves; while it lasts, the backlog is logged every <see cref="WaitLogInterval"/> on the host's clock.
/// </para>
/// </summary>
public sealed class CoreLifetime : IHostedLifecycleService
{
    /// <summary>
    /// How often the wait for saves reports what it is waiting for.
    /// </summary>
    public static readonly TimeSpan WaitLogInterval = TimeSpan.FromSeconds(5);

    private readonly PersistenceContexts _contexts;
    private readonly WriteBehindQueue _queue;
    private readonly AccountOwner _accounts;
    private readonly RealmOwner _realm;
    private readonly IMapScheduler _scheduler;
    private readonly MapDirectory _maps;
    private readonly TimeProvider _time;
    private readonly TimeSpan _firstBlockTimeout;
    private readonly KestrelServerOptions _kestrel;
    private readonly GatewayOptions _gateway;
    private readonly ILogger<CoreLifetime> _logger;

    /// <summary>
    /// Create the lifetime over the core's parts.
    /// </summary>
    public CoreLifetime(
        PersistenceContexts contexts,
        WriteBehindQueue queue,
        AccountOwner accounts,
        RealmOwner realm,
        IMapScheduler scheduler,
        MapDirectory maps,
        TimeProvider time,
        IOptions<PersistenceOptions> persistence,
        IOptions<KestrelServerOptions> kestrel,
        IOptions<GatewayOptions> gateway,
        ILogger<CoreLifetime> logger)
    {
        _contexts = contexts;
        _queue = queue;
        _accounts = accounts;
        _realm = realm;
        _scheduler = scheduler;
        _maps = maps;
        _time = time;
        _firstBlockTimeout = persistence.Value.FirstIdBlockTimeout;
        _kestrel = kestrel.Value;
        _gateway = gateway.Value;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task StartingAsync(CancellationToken cancellationToken)
    {
        List<string> pending = await PendingAsync(cancellationToken);
        if (pending.Count > 0)
            throw new InvalidOperationException($"The database has pending migrations (schemas: {string.Join(", ", pending)}): run `NexusTomorrow.Server migrate` first.");

        _queue.Start();
        try
        {
            // The block is posted before the realm owner starts and before Kestrel listens, so no connection can ask for a character first.
            var taken = new TaskCompletionSource<IdBlock>(TaskCreationOptions.RunContinuationsAsynchronously);
            _queue.Enqueue(new TakeIdBlockWork(taken.SetResult));
            _realm.Post(new IdBlockTaken(await WaitForFirstBlockAsync(taken.Task, cancellationToken)));
            _accounts.Start();
            _realm.Start();
            _scheduler.Start();
            GatewayEndpoints.Listen(_kestrel, _gateway);
        }
        catch
        {
            // Nothing a player did is waiting in the queue yet, so the start-up work it still holds is dropped rather than awaited.
            await _scheduler.DisposeAsync();
            await _realm.StopAsync();
            await _accounts.StopAsync();
            await _queue.AbandonAsync();
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task StoppedAsync(CancellationToken cancellationToken)
    {
        try
        {
            // A map stops on its next tick, so the scheduler runs until every map has handed its characters back; the realm then
            // handles those returns, and the queue is disposed only after the maps' saves are in it.
            try
            {
                await _maps.StopAsync();
            }
            finally
            {
                await _scheduler.DisposeAsync();
            }

            await _realm.StopAsync();
            await _accounts.StopAsync();
        }
        finally
        {
            await AwaitSavesAsync();
        }
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task<IdBlock> WaitForFirstBlockAsync(Task<IdBlock> taken, CancellationToken cancellationToken)
    {
        try
        {
            return await taken.WaitAsync(_firstBlockTimeout, _time, cancellationToken);
        }
        catch (TimeoutException)
        {
            throw new InvalidOperationException(
                $"The host could not take the first character id block within {_firstBlockTimeout}: is the database writable?");
        }
    }

    // The token of a host stop is not passed on: it carries the shutdown timeout, and the saves wait for the database however long it
    // takes.
    private async Task AwaitSavesAsync()
    {
        Task disposed = _queue.DisposeAsync().AsTask();
        using var timer = new PeriodicTimer(WaitLogInterval, _time);
        while (!disposed.IsCompleted)
        {
            Task tick = timer.WaitForNextTickAsync(CancellationToken.None).AsTask();
            if (await Task.WhenAny(disposed, tick) == disposed)
                break;

            ServerLog.WaitingForSaves(_logger, _queue.Backlog, _queue.IsAvailable);
        }

        await disposed;
    }

    private async Task<List<string>> PendingAsync(CancellationToken cancellationToken)
    {
        List<string> pending = [];
        await using (AccountContext account = _contexts.CreateAccount())
        {
            if (await HasPendingAsync(account, cancellationToken))
                pending.Add(AccountContext.Schema);
        }

        await using (RealmContext realm = _contexts.CreateRealm())
        {
            if (await HasPendingAsync(realm, cancellationToken))
                pending.Add(RealmContext.Schema);
        }

        await using (OpsContext ops = _contexts.CreateOps())
        {
            if (await HasPendingAsync(ops, cancellationToken))
                pending.Add(OpsContext.Schema);
        }

        return pending;
    }

    private static async Task<bool> HasPendingAsync(DbContext context, CancellationToken cancellationToken) =>
        (await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any();
}
