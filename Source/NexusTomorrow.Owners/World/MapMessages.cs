using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Owners.World;

/// <summary>
/// A message a map owner handles.
/// </summary>
public abstract record MapMessage;

/// <summary>
/// A map message that concerns one client session.
/// </summary>
public abstract record SessionMessage(IWorldSession Session) : MapMessage;

/// <summary>
/// The realm handing a character to the map (P6): the map holds it until it leaves, then hands it back to
/// <paramref name="ReturnTo"/>.
/// </summary>
public sealed record CharacterArrived(IWorldSession Session, long AccountId, CharacterRecord Character, IRealmInbox ReturnTo)
    : SessionMessage(Session);

/// <summary>
/// The session's connection ended.
/// </summary>
public sealed record ClientLeft(IWorldSession Session) : SessionMessage(Session);
