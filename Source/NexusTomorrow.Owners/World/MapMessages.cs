using System.Numerics;
using NexusTomorrow.Network.World.Entity;
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

/// <summary>
/// The newest finite position the client sent for its unit, in world coordinates, and its newest finite facing, the yaw in radians;
/// either is null when the message carried none, a non-finite one counting as none.
/// </summary>
public readonly record struct MovementSample(Vector3? Position, float? Facing);

/// <summary>
/// One client movement batch for the players who see the mover: the client's own time for it (<c>ClientMovement.ClientTime</c>) and
/// the commands it sent in order, only the arms the relay carries and each with finite values. The commands are the decoded client
/// objects, which nothing writes after the decode.
/// </summary>
public sealed record RelayBatch(uint Time, Server0262Part5[] Commands);

/// <summary>
/// The client moved its unit: the newest position and facing for the map to keep, and the batch to relay, or null when the message
/// carried no command the relay carries.
/// </summary>
public sealed record ClientMoved(IWorldSession Session, MovementSample Sample, RelayBatch? Batch) : SessionMessage(Session);

/// <summary>
/// The client asked to leave the world (<paramref name="Begin"/> true) or cancelled a request still counting down (<paramref name="Begin"/> false).
/// </summary>
public sealed record ClientLogoutRequested(IWorldSession Session, bool Begin) : SessionMessage(Session);

/// <summary>
/// The client confirmed its logout request; on an exit it then quits.
/// </summary>
public sealed record ClientLogoutConfirmed(IWorldSession Session) : SessionMessage(Session);

/// <summary>
/// A line the realm checked and passed on for its speaker's map to say: the speaker's session and character, the client's own id
/// for the line, and the line.
/// </summary>
public sealed record LocalChat(IWorldSession Session, ulong CharacterId, ushort ChatId, string Text) : SessionMessage(Session);
