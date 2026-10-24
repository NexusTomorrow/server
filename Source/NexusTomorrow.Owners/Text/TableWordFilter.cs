using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable;
using NexusTomorrow.GameTable.Model;
using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Owners.Text;

/// <summary>
/// The word filter read once, at construction, from the operator's own extracted client table (<c>WordFilter.tbl</c> under
/// <see cref="ContentOptions.ClientDataPath"/>). It takes every row, whatever its class and language: the Strict class holds every
/// class, and a server serves every language. With no path, or the table missing or unreadable, it logs a warning and allows every
/// text.
/// </summary>
public sealed class TableWordFilter : IWordFilter
{
    private readonly WordFilter _filter;

    /// <summary>
    /// Read the table named by <paramref name="options"/>.
    /// </summary>
    public TableWordFilter(IOptions<ContentOptions> options, ILogger<TableWordFilter> logger)
    {
        _filter = new WordFilter(Load(options.Value.ClientDataPath, logger), logger);
    }

    /// <inheritdoc/>
    public bool Allows(ReadOnlySpan<char> text) => _filter.Allows(text);

    private static (uint Type, string Text)[] Load(string clientDataPath, ILogger logger)
    {
        if (string.IsNullOrEmpty(clientDataPath))
        {
            TextLog.NoClientDataPath(logger);
            return [];
        }

        string path = Path.Combine(clientDataPath, "tbl", "WordFilter.tbl");
        if (!File.Exists(path))
        {
            TextLog.TableUnavailable(logger, path, null);
            return [];
        }

        try
        {
            return [.. new GameTable<WordFilterEntry>(path).Entries.Select(e => (e.WordFilterTypeEnum, e.Filter))];
        }
        catch (Exception exception)
        {
            // Any failure reading the player's table leaves the filter empty and the start going on. GameTable is not hardened
            // against a damaged file, which can fail it with anything from a file the server may not open to a repeated id.
            TextLog.TableUnavailable(logger, path, exception);
            return [];
        }
    }
}
