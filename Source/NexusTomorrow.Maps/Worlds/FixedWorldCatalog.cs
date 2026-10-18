using System.Collections.Frozen;

namespace NexusTomorrow.Maps.Worlds;

/// <summary>
/// A catalog that holds exactly the worlds it is given, for the harness and tests.
/// </summary>
public sealed class FixedWorldCatalog : IWorldCatalog
{
    private readonly FrozenDictionary<ushort, WorldKind> _worlds;

    /// <summary>
    /// Hold <paramref name="worlds"/>.
    /// </summary>
    public FixedWorldCatalog(IEnumerable<KeyValuePair<ushort, WorldKind>> worlds)
    {
        _worlds = worlds.ToFrozenDictionary();
    }

    /// <inheritdoc/>
    public WorldKind KindOf(ushort worldId) => _worlds.GetValueOrDefault(worldId);
}
