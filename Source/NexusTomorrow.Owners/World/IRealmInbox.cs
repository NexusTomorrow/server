using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Owners.World;

/// <summary>
/// Where messages reach the realm owner. Safe from any thread; false once the realm has stopped.
/// </summary>
public interface IRealmInbox
{
    /// <summary>
    /// Queue <paramref name="message"/>; false once the realm has stopped.
    /// </summary>
    bool Post(RealmMessage message);
}
