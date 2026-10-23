using System.Collections.Frozen;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable;
using NexusTomorrow.GameTable.Model;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// A catalog read once, at construction, from the operator's own extracted client tables (<c>CharacterCreation.tbl</c> and
/// <c>XpPerLevel.tbl</c> under <see cref="ContentOptions.ClientDataPath"/>). With no path, or a table missing or unreadable, it logs
/// a warning and holds no options, so every create is refused.
/// </summary>
public sealed class TableCreationCatalog : ICreationCatalog
{
    private readonly FrozenDictionary<uint, CreationOption> _options;

    /// <summary>
    /// Read the tables named by <paramref name="options"/>.
    /// </summary>
    public TableCreationCatalog(IOptions<ContentOptions> options, ILogger<TableCreationCatalog> logger)
    {
        _options = Load(options.Value.ClientDataPath, logger);
    }

    /// <inheritdoc/>
    public CreationOption? Find(uint creationId) => _options.GetValueOrDefault(creationId);

    private static FrozenDictionary<uint, CreationOption> Load(string clientDataPath, ILogger logger)
    {
        if (string.IsNullOrEmpty(clientDataPath))
        {
            RealmLog.NoClientDataPath(logger);
            return FrozenDictionary<uint, CreationOption>.Empty;
        }

        string creationPath = Path.Combine(clientDataPath, "tbl", "CharacterCreation.tbl");
        string levelPath = Path.Combine(clientDataPath, "tbl", "XpPerLevel.tbl");
        if (!File.Exists(creationPath) || !File.Exists(levelPath))
        {
            RealmLog.TableUnavailable(logger, File.Exists(creationPath) ? levelPath : creationPath, null);
            return FrozenDictionary<uint, CreationOption>.Empty;
        }

        try
        {
            XpPerLevelEntry[] levels = [.. new GameTable<XpPerLevelEntry>(levelPath).Entries.OrderBy(l => l.Id)];
            return new GameTable<CharacterCreationEntry>(creationPath).Entries.ToFrozenDictionary(
                e => e.Id,
                e => new CreationOption(
                    e.Id, (byte)e.RaceId, (byte)e.Sex, (byte)e.ClassId, (uint)e.FactionId, e.Enabled, e.CharacterCreationStartEnum, e.Xp,
                    LevelFor(levels, e.Xp), e.EntitlementIdRequired, e.AccountCurrencyAmountCost, e.CostumeOnly));
        }
        catch (Exception exception)
        {
            // Any failure reading the player's tables leaves the catalog empty and the start going on. GameTable is not hardened
            // against a damaged file, which can fail it with anything from a file the server may not open to a repeated id.
            RealmLog.TableUnavailable(logger, creationPath, exception);
            return FrozenDictionary<uint, CreationOption>.Empty;
        }
    }

    // The id of the last level row whose minimum is at most the option's xp; 1 when no row qualifies.
    private static uint LevelFor(XpPerLevelEntry[] levels, uint xp)
    {
        uint level = 1;
        foreach (XpPerLevelEntry row in levels)
        {
            if (row.MinXpForLevel <= xp)
                level = row.Id;
        }

        return level;
    }
}
