using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway;
using NexusTomorrow.Gateway.Kestrel;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Server;

/// <summary>
/// Brings the core up before Kestrel listens and takes it down after Kestrel has stopped accepting. Start: refuse a schema with pending
/// migrations, start the write-behind queue, hand the realm owner its first character id block, start both owners, then give Kestrel\r\n/// its three listeners (Kestrel binds only after this start, so nothing listens before the core is ready). Stop: stop both
/// owners (each handles what it has queued), then wait for every queued save to commit. Nothing bounds that wait, because a bound
/// would drop saves; while it lasts, the backlog is logged every <see cref="WaitLogInterval"/> on the host's clock.
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
    private readonly TimeProvider _time;
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
        TimeProvider time,
        IOptions<KestrelServerOptions> kestrel,
        IOptions<GatewayOptions> gateway,
        ILogger<CoreLifetime> logger)
    {
        _contexts = contexts;
        _queue = queue;
        _accounts = accounts;
        _realm = realm;
        _time = time;
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

        // The block is posted before the realm owner starts and before Kestrel listens, so no connection can ask for a character first.
        var taken = new TaskCompletionSource<IdBlock>(TaskCreationOptions.RunContinuationsAsynchronously);
        _queue.Enqueue(new TakeIdBlockWork(taken.SetResult));
        _realm.Post(new IdBlockTaken(await taken.Task.WaitAsync(cancellationToken)));
        _accounts.Start();
        _realm.Start();
        GatewayEndpoints.Listen(_kestrel, _gateway);
    }

    /// <inheritdoc/>
    public async Task StoppedAsync(CancellationToken cancellationToken)
    {
        await _realm.StopAsync();
        await _accounts.StopAsync();

        // The token is not passed on: it carries the shutdown timeout, and the saves wait for the database however long it takes.
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

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

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
