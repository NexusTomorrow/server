using System.Collections.Frozen;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// A catalog over options given up front.
/// </summary>
public sealed class FixedCreationCatalog : ICreationCatalog
{
    private readonly FrozenDictionary<uint, CreationOption> _options;

    /// <summary>
    /// Create a catalog of <paramref name="options"/>; a later option with the same id replaces an earlier one.
    /// </summary>
    public FixedCreationCatalog(IEnumerable<CreationOption> options)
    {
        _options = options.GroupBy(o => o.Id).ToFrozenDictionary(g => g.Key, g => g.Last());
    }

    /// <inheritdoc/>
    public CreationOption? Find(uint creationId) => _options.GetValueOrDefault(creationId);
}
