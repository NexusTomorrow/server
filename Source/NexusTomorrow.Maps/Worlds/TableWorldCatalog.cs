using System.Collections.Frozen;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable;
using NexusTomorrow.Owners.Realm;
using WorldRow = NexusTomorrow.GameTable.Model.WorldEntry;

namespace NexusTomorrow.Maps.Worlds;

/// <summary>
/// A catalog read once, at construction, from the operator's own extracted <c>World.tbl</c> under
/// <see cref="ContentOptions.ClientDataPath"/>. With no path, or a table missing or unreadable, it logs a warning and holds no
/// world, so no map is hosted.
/// </summary>
public sealed class TableWorldCatalog : IWorldCatalog
{
    private readonly FrozenDictionary<ushort, WorldKind> _worlds;

    /// <summary>
    /// Read the table named by <paramref name="options"/>.
    /// </summary>
    public TableWorldCatalog(IOptions<ContentOptions> options, ILogger<TableWorldCatalog> logger)
    {
        _worlds = Load(options.Value.ClientDataPath, logger);
    }

    /// <inheritdoc/>
    public WorldKind KindOf(ushort worldId) => _worlds.GetValueOrDefault(worldId);

    private static FrozenDictionary<ushort, WorldKind> Load(string clientDataPath, ILogger logger)
    {
        if (string.IsNullOrEmpty(clientDataPath))
        {
            MapLog.NoClientDataPath(logger);
            return FrozenDictionary<ushort, WorldKind>.Empty;
        }

        string path = Path.Combine(clientDataPath, "tbl", "World.tbl");
        if (!File.Exists(path))
        {
            MapLog.WorldTableUnavailable(logger, path, null);
            return FrozenDictionary<ushort, WorldKind>.Empty;
        }

        try
        {
            var worlds = new Dictionary<ushort, WorldKind>();
            foreach (WorldRow row in new GameTable<WorldRow>(path).Entries)
            {
                // A world id travels in 15 bits (EnterWorld.WorldId), so every row the client can enter fits 16.
                if (row.Id <= ushort.MaxValue)
                    worlds[(ushort)row.Id] = WorldKinds.Classify((uint)row.Type, row.Flags);
            }

            return worlds.ToFrozenDictionary();
        }
        catch (Exception exception)
        {
            // Any failure reading the player's table leaves the catalog empty and the start going on. GameTable is not hardened
            // against a damaged file, which can fail it with anything from a file the server may not open to a repeated id.
            MapLog.WorldTableUnavailable(logger, path, exception);
            return FrozenDictionary<ushort, WorldKind>.Empty;
        }
    }
}
