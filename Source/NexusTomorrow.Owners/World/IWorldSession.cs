namespace NexusTomorrow.Owners.World;

/// <summary>
/// A client connection that can be routed to a map. Every member is safe to call from any thread.
/// </summary>
public interface IWorldSession : IClientSession
{
    /// <summary>
    /// Route the session's in-world messages to <paramref name="map"/>. False when the connection has already ended; once this
    /// returned true, the connection posts <see cref="ClientLeft"/> to the map when it ends, unless <see cref="LeaveMap"/> came first.
    /// </summary>
    bool TryEnterMap(IMapInbox map);

    /// <summary>
    /// Stop routing to <paramref name="map"/>. Naming a map the session is not routed to changes nothing.
    /// </summary>
    void LeaveMap(IMapInbox map);
}
