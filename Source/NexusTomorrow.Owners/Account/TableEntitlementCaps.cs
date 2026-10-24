using System.Collections.Frozen;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable;
using NexusTomorrow.GameTable.Model;
using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The caps read once, at construction, from the operator's own extracted client table (<c>Entitlement.tbl</c> under
/// <see cref="ContentOptions.ClientDataPath"/>). With no path, or the table missing or unreadable, it logs a warning and names no
/// entitlement.
/// </summary>
public sealed class TableEntitlementCaps : IEntitlementCaps
{
    private readonly FrozenDictionary<uint, uint> _caps;

    /// <summary>
    /// Read the table named by <paramref name="options"/>.
    /// </summary>
    public TableEntitlementCaps(IOptions<ContentOptions> options, ILogger<TableEntitlementCaps> logger)
    {
        _caps = Load(options.Value.ClientDataPath, logger);
    }

    /// <inheritdoc/>
    public uint? MaxCountOf(uint entitlementId) => _caps.TryGetValue(entitlementId, out uint cap) ? cap : null;

    private static FrozenDictionary<uint, uint> Load(string clientDataPath, ILogger logger)
    {
        if (string.IsNullOrEmpty(clientDataPath))
        {
            AccountLog.NoClientDataPath(logger, "Entitlement.tbl");
            return FrozenDictionary<uint, uint>.Empty;
        }

        string path = Path.Combine(clientDataPath, "tbl", "Entitlement.tbl");
        if (!File.Exists(path))
        {
            AccountLog.TableUnavailable(logger, path, null);
            return FrozenDictionary<uint, uint>.Empty;
        }

        try
        {
            return new GameTable<EntitlementEntry>(path).Entries.ToFrozenDictionary(e => e.Id, e => e.MaxCount);
        }
        catch (Exception exception)
        {
            // Any failure reading the player's table leaves the caps empty and the start going on. GameTable is not hardened against
            // a damaged file, which can fail it with anything from a file the server may not open to a repeated id.
            AccountLog.TableUnavailable(logger, path, exception);
            return FrozenDictionary<uint, uint>.Empty;
        }
    }
}
