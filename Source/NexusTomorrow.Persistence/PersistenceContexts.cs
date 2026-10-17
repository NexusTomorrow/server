using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Persistence;

/// <summary>
/// The three contexts' pooled factories. Safe to share across threads; each created context is used by one caller and disposed.
/// </summary>
public sealed class PersistenceContexts
{
    private readonly IDbContextFactory<AccountContext> _account;
    private readonly IDbContextFactory<RealmContext> _realm;
    private readonly IDbContextFactory<OpsContext> _ops;

    /// <summary>
    /// Wrap the three factories.
    /// </summary>
    public PersistenceContexts(IDbContextFactory<AccountContext> account, IDbContextFactory<RealmContext> realm, IDbContextFactory<OpsContext> ops)
    {
        _account = account;
        _realm = realm;
        _ops = ops;
    }

    /// <summary>
    /// Pooled factories for <paramref name="connectionString"/>. Creating them does not connect.
    /// </summary>
    public static PersistenceContexts Create(string connectionString) => new(
        new PooledDbContextFactory<AccountContext>(SchemaConventions.Options<AccountContext>(connectionString, AccountContext.Schema)),
        new PooledDbContextFactory<RealmContext>(SchemaConventions.Options<RealmContext>(connectionString, RealmContext.Schema)),
        new PooledDbContextFactory<OpsContext>(SchemaConventions.Options<OpsContext>(connectionString, OpsContext.Schema)));

    /// <summary>
    /// A new <see cref="AccountContext"/>; the caller disposes it.
    /// </summary>
    public AccountContext CreateAccount() => _account.CreateDbContext();

    /// <summary>
    /// A new <see cref="RealmContext"/>; the caller disposes it.
    /// </summary>
    public RealmContext CreateRealm() => _realm.CreateDbContext();

    /// <summary>
    /// A new <see cref="OpsContext"/>; the caller disposes it.
    /// </summary>
    public OpsContext CreateOps() => _ops.CreateDbContext();

    /// <summary>
    /// Apply every schema's pending migrations.
    /// </summary>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using (AccountContext account = CreateAccount())
            await account.Database.MigrateAsync(cancellationToken);
        await using (RealmContext realm = CreateRealm())
            await realm.Database.MigrateAsync(cancellationToken);
        await using (OpsContext ops = CreateOps())
            await ops.Database.MigrateAsync(cancellationToken);
    }
}
