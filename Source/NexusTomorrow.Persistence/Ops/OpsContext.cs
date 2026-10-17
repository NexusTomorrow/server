using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Ops;

/// <summary>
/// The <c>ops</c> schema: the audit trail of admin actions.
/// </summary>
public sealed class OpsContext : DbContext
{
    /// <summary>
    /// The schema this context owns, which also holds its migration history.
    /// </summary>
    public const string Schema = "ops";

    /// <summary>
    /// Create a context with <paramref name="options"/>.
    /// </summary>
    public OpsContext(DbContextOptions<OpsContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// <c>ops.audit</c>.
    /// </summary>
    public DbSet<AuditRow> Audit => Set<AuditRow>();

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) => SchemaConventions.Apply(configurationBuilder);

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.Entity<AuditRow>(audit =>
        {
            audit.ToTable("audit");
            audit.Property(a => a.Id).UseIdentityAlwaysColumn();
            audit.Property(a => a.Actor).HasMaxLength(100);
            audit.Property(a => a.FrontEnd).HasMaxLength(20);
            audit.Property(a => a.Action).HasMaxLength(100);
            audit.Property(a => a.Target).HasMaxLength(200);
            audit.Property(a => a.Details).HasMaxLength(2000);
            audit.Property(a => a.RemoteAddress).HasMaxLength(64);
            audit.Property(a => a.Outcome).HasMaxLength(20);
        });
    }
}
