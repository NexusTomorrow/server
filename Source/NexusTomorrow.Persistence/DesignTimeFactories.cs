using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Persistence;

/// <summary>
/// Builds an <see cref="AccountContext"/> for the EF tools. They build the model from it and never connect.
/// </summary>
public sealed class AccountContextDesignFactory : IDesignTimeDbContextFactory<AccountContext>
{
    private const string DesignConnection = "Host=localhost;Database=nexustomorrow_design";

    /// <inheritdoc/>
    public AccountContext CreateDbContext(string[] args) => new(SchemaConventions.Options<AccountContext>(DesignConnection, AccountContext.Schema));
}

/// <summary>
/// Builds a <see cref="RealmContext"/> for the EF tools. They build the model from it and never connect.
/// </summary>
public sealed class RealmContextDesignFactory : IDesignTimeDbContextFactory<RealmContext>
{
    private const string DesignConnection = "Host=localhost;Database=nexustomorrow_design";

    /// <inheritdoc/>
    public RealmContext CreateDbContext(string[] args) => new(SchemaConventions.Options<RealmContext>(DesignConnection, RealmContext.Schema));
}

/// <summary>
/// Builds an <see cref="OpsContext"/> for the EF tools. They build the model from it and never connect.
/// </summary>
public sealed class OpsContextDesignFactory : IDesignTimeDbContextFactory<OpsContext>
{
    private const string DesignConnection = "Host=localhost;Database=nexustomorrow_design";

    /// <inheritdoc/>
    public OpsContext CreateDbContext(string[] args) => new(SchemaConventions.Options<OpsContext>(DesignConnection, OpsContext.Schema));
}
