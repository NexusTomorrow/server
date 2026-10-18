namespace NexusTomorrow.Maps.Worlds;

/// <summary>
/// What the server does with each world; read once at start-up, so safe from any thread.
/// </summary>
public interface IWorldCatalog
{
    /// <summary>
    /// The kind of <paramref name="worldId"/>; <see cref="WorldKind.Unknown"/> for a world the catalog does not hold.
    /// </summary>
    WorldKind KindOf(ushort worldId);
}
