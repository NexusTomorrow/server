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

/// <summary>
/// The client loaded the world (<c>WorldReady</c>), standing in the zone it names; 0 names none.
/// </summary>
public sealed record ClientWorldReady(IWorldSession Session, ushort ZoneId) : SessionMessage(Session);

/// <summary>
/// The client answered a movement control grant with its ticket (<c>MovementControlReceipt</c>).
/// </summary>
public sealed record ClientControlReceipt(IWorldSession Session, uint Ticket) : SessionMessage(Session);

/// <summary>
/// The client reported the zone its unit stands in (<c>ZoneChange</c>).
/// </summary>
public sealed record ClientZoneChanged(IWorldSession Session, ushort ZoneId) : SessionMessage(Session);
