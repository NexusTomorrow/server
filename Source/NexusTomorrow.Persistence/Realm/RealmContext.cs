using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// The <c>realm</c> schema: characters at rest, with their look.
/// </summary>
public sealed class RealmContext : DbContext
{
    /// <summary>
    /// The schema this context owns, which also holds its migration history.
    /// </summary>
    public const string Schema = "realm";

    /// <summary>
    /// How many character ids one <c>nextval</c> of <see cref="CharacterIdSequence"/> hands out.
    /// </summary>
    public const int CharacterIdBlock = 1000;

    /// <summary>
    /// The sequence character id blocks come from; it steps by <see cref="CharacterIdBlock"/>.
    /// </summary>
    public const string CharacterIdSequence = "character_id_seq";

    /// <summary>
    /// The unique index that keeps live characters' names apart; a create or rename that hits it is refused as a name taken.
    /// </summary>
    public const string LiveNameIndex = "ix_character_name_live";

    /// <summary>
    /// Create a context with <paramref name="options"/>.
    /// </summary>
    public RealmContext(DbContextOptions<RealmContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// <c>realm.character</c>.
    /// </summary>
    public DbSet<CharacterRow> Characters => Set<CharacterRow>();

    /// <summary>
    /// <c>realm.character_customization</c>.
    /// </summary>
    public DbSet<CharacterCustomizationRow> Customizations => Set<CharacterCustomizationRow>();

    /// <summary>
    /// <c>realm.character_bone</c>.
    /// </summary>
    public DbSet<CharacterBoneRow> Bones => Set<CharacterBoneRow>();

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) => SchemaConventions.Apply(configurationBuilder);

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.HasSequence<long>(CharacterIdSequence).StartsAt(1).IncrementsBy(CharacterIdBlock);
        modelBuilder.Entity<CharacterRow>(character =>
        {
            character.ToTable("character");
            character.Property(c => c.Id).ValueGeneratedNever();
            character.Property(c => c.Name).HasMaxLength(31).UseCollation(SchemaConventions.CaseInsensitive);
            character.HasIndex(c => c.Name).IsUnique().HasFilter("deleted_at IS NULL").HasDatabaseName(LiveNameIndex);
            character.HasIndex(c => c.AccountId);
            character.HasMany(c => c.Customizations).WithOne().HasForeignKey(c => c.CharacterId);
            character.HasMany(c => c.Bones).WithOne().HasForeignKey(b => b.CharacterId);
        });
        modelBuilder.Entity<CharacterCustomizationRow>(customization =>
        {
            customization.ToTable("character_customization");
            customization.HasKey(c => new { c.CharacterId, c.Label });
        });
        modelBuilder.Entity<CharacterBoneRow>(bone =>
        {
            bone.ToTable("character_bone");
            bone.HasKey(b => new { b.CharacterId, b.BoneIndex });
        });
    }
}
