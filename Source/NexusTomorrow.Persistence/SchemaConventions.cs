using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence;

/// <summary>
/// What every context shares: the unsigned-to-bigint conversion, the case-insensitive collation, and how a context connects.
/// </summary>
public static class SchemaConventions
{
    /// <summary>
    /// The case-insensitive collation's name, unqualified: it lives in <c>public</c>, which every connection's search path holds.
    /// </summary>
    public const string CaseInsensitive = "case_insensitive";

    /// <summary>
    /// Creates <see cref="CaseInsensitive"/>. Each schema's first migration runs it, so it exists whichever migration runs first.
    /// </summary>
    public const string CreateCaseInsensitiveCollation =
        "CREATE COLLATION IF NOT EXISTS public.case_insensitive (provider = icu, locale = 'und-u-ks-level2', deterministic = false);";

    private const string HistoryTable = "__ef_migrations_history";

    /// <summary>
    /// Store <see cref="uint"/> and <see cref="ulong"/> as <c>bigint</c>.
    /// </summary>
    public static void Apply(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<uint>().HaveConversion<long>();
        configuration.Properties<ulong>().HaveConversion<long>();
    }

    /// <summary>
    /// The options a context of <typeparamref name="T"/> connects with: Npgsql with its retry strategy, the migration history in the
    /// context's own <paramref name="schema"/>, and snake-case names.
    /// </summary>
    public static DbContextOptions<T> Options<T>(string connectionString, string schema)
        where T : DbContext =>
        new DbContextOptionsBuilder<T>()
            .UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable(HistoryTable, schema);
                npgsql.EnableRetryOnFailure();
            })
            .UseSnakeCaseNamingConvention()
            .Options;
}
