using System.Numerics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable.Text.Static;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Text;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The realm side: each online account's characters, loaded once per world session, the character-select answers, and the character
/// handoff (P6): select loads the character in full and hands it to its world's map, which hands it back when it leaves. An answer that
/// depends on a write is sent only from the write's completion message. A character away from rest is never loaded again until its
/// map hands it back. A chat line from the world is checked here (its channel, the client's chat rule and the word filter) and passed
/// to the map holding the speaker's character, and a command's relocate goes to the map holding its character.
/// </summary>
public sealed class RealmOwner : Owner<RealmMessage>, IRealmInbox
{
    // The highest path id CharacterCreate.Path documents (3 Explorer).
    private const byte LastPath = 3;

    // The client's built-in Say channel (ChatChannelKey.Type).
    private const ushort SayChannelType = 4;

    private readonly IWriteBehind _persistence;
    private readonly ICreationCatalog _catalog;
    private readonly IWordFilter _filter;
    private readonly AccountDefaults _accountDefaults;
    private readonly IMapDirectory _maps;
    private readonly TimeProvider _time;
    private readonly RealmOptions _options;
    private readonly ILogger<RealmOwner> _log;
    private readonly IdBlockAllocator _ids = new(refillBelow: RealmContext.CharacterIdBlock / 2);
    private readonly Dictionary<long, AccountCharacters> _accounts = [];
    private readonly HashSet<string> _pendingNames = new(StringComparer.OrdinalIgnoreCase);

    // Creates enqueued and not yet completed, by account: the cap counts them with the listed characters, whichever session asked,
    // because a session's load cannot see a create that has not committed. Released in Created on every outcome.
    private readonly Dictionary<long, int> _pendingCreates = [];

    // Characters with a rename enqueued and not yet completed: the mark is cleared only on commit, so a second request in the
    // meantime would otherwise pass the RequiresRename check. Released in Renamed on every outcome.
    private readonly HashSet<ulong> _renaming = [];

    // A character away from rest, by character id: loading for a select, or held by a map. A presence leaves through Release (a load
    // whose session closed, a load that found nothing, a world no map hosts, a map that refused the arrival, and the faults of a
    // select or of a load before its arrival was posted), through Returned, and through the fault path of Returned.
    private readonly Dictionary<ulong, Presence> _presences = [];

    // The character each session has on its way in or in the world, by session reference. A session parked behind another's
    // presence holds its character too, so it cannot start a second select.
    private readonly Dictionary<IWorldSession, ulong> _held = new(ReferenceEqualityComparer.Instance);

    // Selects waiting for a character another session holds, by character id: at most one, the newest.
    private readonly Dictionary<ulong, SelectCharacter> _parked = [];

    private long _ticket;

    // Written by the owner, read from any thread.
    private long _droppedChatLines;

    /// <summary>
    /// Create the realm owner.
    /// </summary>
    public RealmOwner(
        IWriteBehind persistence, ICreationCatalog catalog, IWordFilter filter, AccountDefaults accountDefaults, IMapDirectory maps,
        TimeProvider time, IOptions<RealmOptions> options, ILogger<RealmOwner> logger)
        : base("realm", logger)
    {
        _persistence = persistence;
        _catalog = catalog;
        _filter = filter;
        _accountDefaults = accountDefaults;
        _maps = maps;
        _time = time;
        _options = options.Value;
        _log = logger;
    }

    /// <summary>
    /// How many chat lines the realm refused or could not pass to a map.
    /// </summary>
    public long DroppedChatLines => Volatile.Read(ref _droppedChatLines);

    /// <inheritdoc/>
    protected override void Handle(RealmMessage message)
    {
        switch (message)
        {
            case IdBlockTaken taken:
                _ids.Add(taken.Block);
                break;
            case ListCharacters list:
                List(list);
                break;
            case CharactersLoaded loaded:
                if (_accounts.TryGetValue(loaded.AccountId, out AccountCharacters? state) && state.Session == loaded.Session)
                {
                    state.Characters = [.. loaded.Characters];
                    state.Session.Send(BuildList(state.Characters));
                }

                break;
            case CreateCharacter create:
                Create(create);
                break;
            case CharacterCreated created:
                Created(created);
                break;
            case DeleteCharacter delete:
                Delete(delete);
                break;
            case CharacterDeleted deleted:
                Deleted(deleted);
                break;
            case RenameCharacter rename:
                Rename(rename);
                break;
            case CharacterRenamed renamed:
                Renamed(renamed);
                break;
            case SelectCharacter select:
                Select(select);
                break;
            case CharacterLoaded loaded:
                Loaded(loaded);
                break;
            case CharacterReturned returned:
                Returned(returned);
                break;
            case RealmSessionClosed closed:
                Closed(closed);
                break;
            case ChatSaid said:
                Say(said);
                break;
            case RelocateRequested relocate:
                Relocate(relocate);
                break;
        }

        if (_ids.WantsRefill)
        {
            _ids.MarkRefillRequested();
            _persistence.Enqueue(new TakeIdBlockWork(block => Post(new IdBlockTaken(block))));
        }
    }

    /// <inheritdoc/>
    protected override void OnFault(RealmMessage message, Exception exception)
    {
        switch (message)
        {
            case ListCharacters { Session: var session }:
                session.Disconnect("the realm owner failed listing characters");
                break;
            case CreateCharacter { Session: var session }:
                session.Disconnect("the realm owner failed creating a character");
                break;
            case DeleteCharacter { Session: var session }:
                session.Disconnect("the realm owner failed deleting a character");
                break;
            case RenameCharacter { Session: var session }:
                session.Disconnect("the realm owner failed renaming a character");
                break;
            case SelectCharacter { Session: var session, CharacterId: var selectedId }:
                // This session's presence for the character, still loading, may have no load queued behind it and would then hold the
                // character until restart; the faulted session is disconnected, so it is freed without an answer. A load that did
                // queue lands on no matching ticket.
                if (_presences.TryGetValue(selectedId, out Presence? created) && ReferenceEquals(created.Session, session) && created.Loading)
                    Release(created, answerGeneral: false);

                session.Disconnect("the realm owner failed selecting a character");
                break;
            case CharacterLoaded { CharacterId: var characterId, Ticket: var ticket }:
                // Only a presence whose arrival was never posted is released: once the map holds the character, a fault elsewhere in
                // the handler (the id refill) must not free it.
                if (_presences.TryGetValue(characterId, out Presence? loading) && loading.Ticket == ticket && loading.Loading)
                    Release(loading, answerGeneral: true);
                break;
            case CharacterReturned { CharacterId: var returnedId }:
                // Whatever Returned stopped at, the character ends at rest: a presence left over (a parked select's load that never
                // queued) and any select still parked for it are dropped, freed and answered.
                if (_presences.Remove(returnedId, out Presence? left))
                {
                    Free(left.Session, returnedId);
                    if (left.Loading)
                        left.Session.Send(new CharacterSelectFailed { Unknown0 = SelectReasons.General });
                }

                if (_parked.Remove(returnedId, out SelectCharacter? waiting))
                {
                    Free(waiting.Session, returnedId);
                    waiting.Session.Send(new CharacterSelectFailed { Unknown0 = SelectReasons.General });
                }

                break;
            case RelocateRequested { Reply: var reply }:
                reply.TrySetResult(false);
                break;
        }
    }

    private void List(ListCharacters list)
    {
        long id = list.AccountId;
        IClientSession session = list.Session;
        if (!_accounts.TryGetValue(id, out AccountCharacters? state) || state.Session != session)
        {
            _accounts[id] = new AccountCharacters(session);
            _persistence.Enqueue(new LoadCharactersWork(id, characters => Post(new CharactersLoaded(id, session, characters))));
        }
        else if (state.Characters is not null)
        {
            session.Send(BuildList(state.Characters));
        }

        // A load still pending: its completion sends the list.
    }

    private void Create(CreateCharacter create)
    {
        IClientSession session = create.Session;
        CharacterCreate request = create.Request;
        if (LoadedFor(create.AccountId, session) is not { } characters)
        {
            RefuseCreate(session, CreateResults.General);
            return;
        }

        // A repeated look id would make two customization rows under one key (RealmContext's character_customization), and the
        // write would throw on every retry while holding the account's keys.
        CreationOption? option = _catalog.Find(request.CreationId);
        if (option?.IsOfferedTo(_accountDefaults.EntitlementIds, _accountDefaults.CreationLevelLimit) != true || request.Path > LastPath
            || RepeatsAnId(request.LookIds))
        {
            RefuseCreate(session, CreateResults.General);
            return;
        }

        if (!CharacterNames.IsAllowed(request.Name, _filter))
        {
            RefuseCreate(session, CreateResults.NameNotAllowed);
            return;
        }

        if (characters.Count + _pendingCreates.GetValueOrDefault(create.AccountId) >= _options.MaxCharactersPerAccount)
        {
            RefuseCreate(session, CreateResults.AccountFull);
            return;
        }

        if (IsNameInUse(request.Name, characters))
        {
            RefuseCreate(session, CreateResults.NameTaken);
            return;
        }

        if (!_ids.TryTake(out ulong id))
        {
            RefuseCreate(session, CreateResults.General);
            return;
        }

        StartLocation? start = _options.StartLocations.Find(l => l.FactionId == option.FactionId);
        var character = new NewCharacter(
            id, create.AccountId, request.Name, option.Id, option.Race, option.Sex, option.Class, option.FactionId, request.Path,
            option.Level, option.Xp, start?.WorldId ?? 0, start?.ZoneId ?? 0, start is null ? Vector3.Zero : new Vector3(start.X, start.Y, start.Z),
            _time.GetUtcNow(), [.. request.LookIds.Zip(request.LookValues, (label, value) => (label, value))], [.. request.Bones]);
        _pendingNames.Add(request.Name);
        long accountId = create.AccountId;
        _persistence.Enqueue(new CreateCharacterWork(character, result => Post(new CharacterCreated(accountId, session, character, result))));
        _pendingCreates[accountId] = _pendingCreates.GetValueOrDefault(accountId) + 1;
    }

    private void Created(CharacterCreated created)
    {
        _pendingNames.Remove(created.Character.Name);
        if (_pendingCreates.GetValueOrDefault(created.AccountId) <= 1)
            _pendingCreates.Remove(created.AccountId);
        else
            _pendingCreates[created.AccountId]--;

        if (created.Result.Outcome == WorkOutcome.Committed)
        {
            if (_accounts.TryGetValue(created.AccountId, out AccountCharacters? state) && state.Session == created.Session)
                state.Characters?.Add(created.Character.ToSummary());
            created.Session.Send(new CharacterCreateResult { CharacterId = created.Character.Id, Result = CreateResults.Created });
        }
        else
        {
            RefuseCreate(created.Session, created.Result.Constraint == RealmContext.LiveNameIndex ? CreateResults.NameTaken : CreateResults.General);
        }
    }

    private void Delete(DeleteCharacter delete)
    {
        IClientSession session = delete.Session;
        if (_presences.ContainsKey(delete.CharacterId)
            || LoadedFor(delete.AccountId, session)?.Exists(c => c.Id == delete.CharacterId) != true)
        {
            session.Send(new CharacterDeleteResult { Result = DeleteResults.General });
            return;
        }

        long accountId = delete.AccountId;
        ulong characterId = delete.CharacterId;
        _persistence.Enqueue(new DeleteCharacterWork(
            accountId, characterId, _time.GetUtcNow(), result => Post(new CharacterDeleted(accountId, session, characterId, result))));
    }

    private void Deleted(CharacterDeleted deleted)
    {
        if (deleted.Result.Outcome != WorkOutcome.Committed)
        {
            deleted.Session.Send(new CharacterDeleteResult { Result = DeleteResults.General });
            return;
        }

        if (_accounts.TryGetValue(deleted.AccountId, out AccountCharacters? state) && state.Session == deleted.Session)
            state.Characters?.RemoveAll(c => c.Id == deleted.CharacterId);
        deleted.Session.Send(new CharacterDeleteResult { Result = DeleteResults.Deleted });
    }

    private void Rename(RenameCharacter rename)
    {
        IClientSession session = rename.Session;
        List<CharacterSummary>? characters = LoadedFor(rename.AccountId, session);
        CharacterSummary? target = characters?.Find(c => c.Id == rename.CharacterId);
        if (characters is null || target is null)
        {
            RefuseRename(session, RenameResults.InternalError);
            return;
        }

        if (!target.RequiresRename || _renaming.Contains(target.Id))
        {
            RefuseRename(session, RenameResults.NoRenameAvailable);
            return;
        }

        if (!CharacterNames.IsAllowed(rename.NewName, _filter))
        {
            RefuseRename(session, RenameResults.NameNotAllowed);
            return;
        }

        if (IsNameInUse(rename.NewName, characters))
        {
            RefuseRename(session, RenameResults.NameTaken);
            return;
        }

        _pendingNames.Add(rename.NewName);
        long accountId = rename.AccountId;
        ulong characterId = rename.CharacterId;
        string name = rename.NewName;
        _persistence.Enqueue(new RenameCharacterWork(
            accountId, characterId, name, result => Post(new CharacterRenamed(accountId, session, characterId, name, result))));
        _renaming.Add(characterId);
    }

    private void Renamed(CharacterRenamed renamed)
    {
        _pendingNames.Remove(renamed.Name);
        _renaming.Remove(renamed.CharacterId);
        if (renamed.Result.Outcome != WorkOutcome.Committed)
        {
            RefuseRename(renamed.Session, renamed.Result.Constraint == RealmContext.LiveNameIndex ? RenameResults.NameTaken : RenameResults.InternalError);
            return;
        }

        List<CharacterSummary>? characters = LoadedFor(renamed.AccountId, renamed.Session);
        int index = characters?.FindIndex(c => c.Id == renamed.CharacterId) ?? -1;
        if (characters is not null && index >= 0)
            characters[index] = characters[index] with { Name = renamed.Name, RequiresRename = false };

        renamed.Session.Send(new CharacterRenameResult { Result = RenameResults.Renamed, CharacterId = renamed.CharacterId, Name = renamed.Name });
    }

    private void Select(SelectCharacter select)
    {
        CharacterSummary? target = LoadedFor(select.AccountId, select.Session)?.Find(c => c.Id == select.CharacterId);

        byte? refusal = target switch
        {
            null => SelectReasons.General,
            { RequiresRename: true } => SelectReasons.MustRename,
            { Disabled: true } => SelectReasons.Disabled,
            _ => null,
        };
        if (refusal is byte reason)
        {
            select.Session.Send(new CharacterSelectFailed { Unknown0 = reason });
            return;
        }

        // The entry already under way answers.
        if (_held.ContainsKey(select.Session))
            return;

        _held[select.Session] = select.CharacterId;
        if (!_presences.ContainsKey(select.CharacterId))
        {
            StartLoad(select);
            return;
        }

        if (_parked.Remove(select.CharacterId, out SelectCharacter? older))
        {
            Free(older.Session, select.CharacterId);
            older.Session.Send(new CharacterSelectFailed { Unknown0 = SelectReasons.General });
        }

        _parked[select.CharacterId] = select;
    }

    private void StartLoad(SelectCharacter select)
    {
        long ticket = ++_ticket;
        ulong characterId = select.CharacterId;
        _presences[characterId] = new Presence(ticket, characterId, select.AccountId, select.Session);
        _persistence.Enqueue(new LoadCharacterWork(
            characterId, select.AccountId, character => Post(new CharacterLoaded(ticket, characterId, character))));
    }

    private void Loaded(CharacterLoaded loaded)
    {
        if (!_presences.TryGetValue(loaded.CharacterId, out Presence? presence) || presence.Ticket != loaded.Ticket)
            return;

        if (presence.Abandoned)
        {
            Release(presence, answerGeneral: false);
            return;
        }

        if (loaded.Character is not { } character)
        {
            Release(presence, answerGeneral: true);
            return;
        }

        // The loaded record's world, not the list's: the map saves the position it last held.
        ushort world = character.Location.WorldId;
        IMapInbox? map = _maps.Host(world);
        if (map is null || !map.Post(new CharacterArrived(presence.Session, presence.AccountId, character, this)))
        {
            RealmLog.WorldNotHosted(_log, character.Id, world);
            Release(presence, answerGeneral: true);
            return;
        }

        presence.Loading = false;
        presence.Map = map;
    }

    // A line goes on only from the Say channel, within the client's chat rule and the word filter, and from a session whose character
    // a map holds. A refused line is logged by its length and reason only, never its text.
    private void Say(ChatSaid said)
    {
        string? refusal = said.ChannelType != SayChannelType ? "not the Say channel"
            : !UserTextRules.Check(said.Text, UserText.Chat) ? "outside the chat rule"
            : !_filter.Allows(said.Text) ? "a filtered word"
            : null;
        if (refusal is null)
        {
            if (MapOf(said.Session, out ulong characterId) is not { } map)
                refusal = "no character in a map";
            else if (map.Post(new LocalChat(said.Session, characterId, said.ChatId, said.Text)))
                return;
            else
                refusal = "the map has stopped";
        }

        RealmLog.ChatDropped(_log, said.Session.Id, said.Text.Length, refusal);
        Volatile.Write(ref _droppedChatLines, _droppedChatLines + 1);
    }

    // The map holding the character answers; a character loading, at rest, or on a map that has stopped is answered false here.
    private void Relocate(RelocateRequested request)
    {
        IMapInbox? map = _presences.TryGetValue(request.CharacterId, out Presence? presence) ? presence.Map : null;
        if (map is null || !map.Post(new RelocateCharacter(request.CharacterId, request.Position, request.Facing, request.Reply)))
            request.Reply.TrySetResult(false);
    }

    // The map holding the session's character from its arrival on; null while the character loads, waits behind another session's
    // presence, or is at rest.
    private IMapInbox? MapOf(IWorldSession session, out ulong characterId) =>
        _held.TryGetValue(session, out characterId) && _presences.TryGetValue(characterId, out Presence? presence)
            && ReferenceEquals(presence.Session, session)
            ? presence.Map
            : null;

    private void Returned(CharacterReturned returned)
    {
        if (!_presences.Remove(returned.CharacterId, out Presence? presence))
            return;

        Free(presence.Session, returned.CharacterId);
        if (_accounts.TryGetValue(returned.AccountId, out AccountCharacters? state) && state.Characters is { } characters)
        {
            int index = characters.FindIndex(c => c.Id == returned.CharacterId);
            if (index >= 0)
            {
                CharacterLocation at = returned.Location;
                characters[index] = characters[index] with { WorldId = at.WorldId, ZoneId = at.ZoneId, Position = at.Position };
            }
        }

        // The map queued its save before it posted this return, so the load a parked select starts reads it.
        RunParked(returned.CharacterId);
    }

    private void Closed(RealmSessionClosed closed)
    {
        if (_accounts.TryGetValue(closed.AccountId, out AccountCharacters? open) && open.Session == closed.Session)
            _accounts.Remove(closed.AccountId);

        if (!_held.Remove(closed.Session, out ulong characterId))
            return;

        if (_parked.TryGetValue(characterId, out SelectCharacter? parked) && ReferenceEquals(parked.Session, closed.Session))
            _parked.Remove(characterId);

        // The load stays listed so the character is not loaded twice, and is dropped when it lands. A character already in a map
        // comes back through the map, which hears of the closed connection itself.
        if (_presences.TryGetValue(characterId, out Presence? presence) && ReferenceEquals(presence.Session, closed.Session) && presence.Loading)
            presence.Abandoned = true;
    }

    private void Release(Presence presence, bool answerGeneral)
    {
        ulong characterId = presence.CharacterId;
        _presences.Remove(characterId);
        Free(presence.Session, characterId);
        if (answerGeneral)
            presence.Session.Send(new CharacterSelectFailed { Unknown0 = SelectReasons.General });

        RunParked(characterId);
    }

    private void RunParked(ulong characterId)
    {
        if (_parked.Remove(characterId, out SelectCharacter? parked))
            StartLoad(parked);
    }

    private void Free(IWorldSession session, ulong characterId)
    {
        if (_held.TryGetValue(session, out ulong held) && held == characterId)
            _held.Remove(session);
    }

    private List<CharacterSummary>? LoadedFor(long accountId, IClientSession session) =>
        _accounts.TryGetValue(accountId, out AccountCharacters? state) && state.Session == session ? state.Characters : null;

    private bool IsNameInUse(string name, List<CharacterSummary> characters) =>
        _pendingNames.Contains(name) || characters.Exists(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

    private static bool RepeatsAnId(List<uint> ids)
    {
        var seen = new HashSet<uint>(ids.Count);
        foreach (uint id in ids)
        {
            if (!seen.Add(id))
                return true;
        }

        return false;
    }

    private static void RefuseCreate(IClientSession session, byte result) =>
        session.Send(new CharacterCreateResult { CharacterId = 0, Result = result });

    private static void RefuseRename(IClientSession session, byte result) =>
        session.Send(new CharacterRenameResult { Result = result });

    private CharacterList BuildList(IReadOnlyList<CharacterSummary> characters)
    {
        var list = new CharacterList
        {
            // The declaration's Windows file time: 100 ns ticks since 1601.
            ServerTime = (ulong)_time.GetUtcNow().ToFileTime(),
            RealmId = _options.Id,
        };

        foreach (CharacterSummary summary in characters)
        {
            var character = new CharacterList.Character
            {
                CharacterId = summary.Id,
                Name = summary.Name,
                GenderId = summary.Sex,
                RaceId = summary.Race,
                ClassId = summary.Class,
                Unknown5 = summary.FactionId,
                Unknown6 = summary.Level,
                WorldId = summary.WorldId,
                ZoneId = summary.ZoneId,
                Position = summary.Position,
                Unknown15 = summary.Path,
                Unknown16 = summary.Disabled,
                Unknown17 = summary.RequiresRename,
            };
            character.Bones.AddRange(summary.Bones);
            list.Characters.Add(character);
        }

        return list;
    }

    private sealed class Presence
    {
        public Presence(long ticket, ulong characterId, long accountId, IWorldSession session)
        {
            Ticket = ticket;
            CharacterId = characterId;
            AccountId = accountId;
            Session = session;
        }

        public long Ticket { get; }

        public ulong CharacterId { get; }

        public long AccountId { get; }

        public IWorldSession Session { get; }

        // True until the arrival is posted to the map.
        public bool Loading { get; set; } = true;

        // The map the arrival was posted to, from then until the character is back at rest; null before.
        public IMapInbox? Map { get; set; }

        // The session closed while the load was out: the load is dropped when it lands.
        public bool Abandoned { get; set; }
    }

    private sealed class AccountCharacters
    {
        public AccountCharacters(IClientSession session)
        {
            Session = session;
        }

        public IClientSession Session { get; }

        public List<CharacterSummary>? Characters { get; set; }
    }
}
