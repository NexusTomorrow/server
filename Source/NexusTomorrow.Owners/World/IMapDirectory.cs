namespace NexusTomorrow.Owners.World;

/// <summary>
/// Which map hosts each world. Safe from any thread.
/// </summary>
public interface IMapDirectory
{
    /// <summary>
    /// The map hosting <paramref name="worldId"/>, made on first use; null when the server hosts no map for it.
    /// </summary>
    IMapInbox? Host(ushort worldId);
}
