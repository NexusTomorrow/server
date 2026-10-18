using System.Collections.Immutable;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// A message the realm owner handles.
/// </summary>
public abstract record RealmMessage;

/// <summary>
/// The client asked for its character list.
/// </summary>
public sealed record ListCharacters(long AccountId, IClientSession Session) : RealmMessage;

/// <summary>
/// The client asked to create a character.
/// </summary>
public sealed record CreateCharacter(long AccountId, IClientSession Session, CharacterCreate Request) : RealmMessage;

/// <summary>
/// The client asked to delete a character.
/// </summary>
public sealed record DeleteCharacter(long AccountId, IClientSession Session, ulong CharacterId) : RealmMessage;

/// <summary>
/// The client asked to rename a character.
/// </summary>
public sealed record RenameCharacter(long AccountId, IClientSession Session, ulong CharacterId, string NewName) : RealmMessage;

/// <summary>
/// The client asked to enter the world as a character.
/// </summary>
public sealed record SelectCharacter(long AccountId, IClientSession Session, ulong CharacterId) : RealmMessage;

/// <summary>
/// A world session ended; its account's cached characters go with it.
/// </summary>
public sealed record RealmSessionClosed(long AccountId, IClientSession Session) : RealmMessage;

/// <summary>
/// A map handed a character back, standing where it left the world, its save already queued when it changed.
/// </summary>
public sealed record CharacterReturned(long AccountId, ulong CharacterId, CharacterLocation Location) : RealmMessage;

/// <summary>
/// A block of character ids came off the database.
/// </summary>
public sealed record IdBlockTaken(IdBlock Block) : RealmMessage;

internal sealed record CharactersLoaded(long AccountId, IClientSession Session, ImmutableArray<CharacterSummary> Characters) : RealmMessage;

internal sealed record CharacterCreated(long AccountId, IClientSession Session, NewCharacter Character, WorkResult Result) : RealmMessage;

internal sealed record CharacterDeleted(long AccountId, IClientSession Session, ulong CharacterId, WorkResult Result) : RealmMessage;

internal sealed record CharacterRenamed(long AccountId, IClientSession Session, ulong CharacterId, string Name, WorkResult Result) : RealmMessage;
