using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// The <c>account</c> schema: logins and their suspensions.
/// </summary>
public sealed class AccountContext : DbContext
{
    /// <summary>
    /// The schema this context owns, which also holds its migration history.
    /// </summary>
    public const string Schema = "account";

    /// <summary>
    /// Create a context with <paramref name="options"/>.
    /// </summary>
    public AccountContext(DbContextOptions<AccountContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// <c>account.account</c>.
    /// </summary>
    public DbSet<AccountRow> Accounts => Set<AccountRow>();

    /// <summary>
    /// <c>account.suspension</c>.
    /// </summary>
    public DbSet<SuspensionRow> Suspensions => Set<SuspensionRow>();

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) => SchemaConventions.Apply(configurationBuilder);

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.Entity<AccountRow>(account =>
        {
            account.ToTable("account");
            account.Property(a => a.Id).UseIdentityAlwaysColumn();
            account.Property(a => a.Email).HasMaxLength(254).UseCollation(SchemaConventions.CaseInsensitive);
            account.HasIndex(a => a.Email).IsUnique();
        });
        modelBuilder.Entity<SuspensionRow>(suspension =>
        {
            suspension.ToTable("suspension");
            suspension.Property(s => s.Id).UseIdentityAlwaysColumn();
            suspension.Property(s => s.Reason).HasMaxLength(200);
            suspension.Property(s => s.CreatedBy).HasMaxLength(100);
            suspension.HasOne<AccountRow>().WithMany().HasForeignKey(s => s.AccountId);
        });
    }
}
