namespace NexusTomorrow.Maps.Visibility;

/// <summary>
/// Whether one player may see another. The client draws every unit it is sent, whatever its phase or stealth, so hiding a unit from
/// a viewer is the server's: a player the predicate hides is never created for that viewer, and one it starts hiding is destroyed
/// there at the next replication flush. A map asks it on its own thread at each flush, for each pair in range.
/// </summary>
public interface IVisibilityPredicate
{
    /// <summary>
    /// True when the viewer's character may see the target's.
    /// </summary>
    bool CanSee(ulong viewerCharacterId, ulong targetCharacterId);
}
