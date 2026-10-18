using System.Numerics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The realm side: each online account's characters, loaded once per world session, and the character-select answers. An answer that
/// depends on a write is sent only from the write's completion message.
/// </summary>
public sealed class RealmOwner : Owner<RealmMessage>
{
    // The highest path id CharacterCreate.Path documents (3 Explorer).
    private const byte LastPath = 3;

    private readonly IWriteBehind _persistence;
    private readonly ICreationCatalog _catalog;
    private readonly TimeProvider _time;
    private readonly RealmOptions _options;
    private readonly IdBlockAllocator _ids = new(refillBelow: RealmContext.CharacterIdBlock / 2);
    private readonly Dictionary<long, AccountCharacters> _accounts = [];
    private readonly HashSet<string> _pendingNames = new(StringComparer.OrdinalIgnoreCase);

    // Creates enqueued and not yet completed, by account: the cap counts them with the listed characters, whichever session asked,
    // because a session's load cannot see a create that has not committed. Released in Created on every outcome.
    private readonly Dictionary<long, int> _pendingCreates = [];

    // Characters with a rename enqueued and not yet completed: the mark is cleared only on commit, so a second request in the
    // meantime would otherwise pass the RequiresRename check. Released in Renamed on every outcome.
    private readonly HashSet<ulong> _renaming = [];

    /// <summary>
    /// Create the realm owner.
    /// </summary>
    public RealmOwner(
        IWriteBehind persistence, ICreationCatalog catalog, TimeProvider time, IOptions<RealmOptions> options, ILogger<RealmOwner> logger)
        : base("realm", logger)
    {
        _persistence = persistence;
        _catalog = catalog;
        _time = time;
        _options = options.Value;
    }

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
            case RealmSessionClosed closed:
                if (_accounts.TryGetValue(closed.AccountId, out AccountCharacters? open) && open.Session == closed.Session)
                    _accounts.Remove(closed.AccountId);
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
            case SelectCharacter { Session: var session }:
                session.Disconnect("the realm owner failed selecting a character");
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
        if (option is not { IsOffered: true } || request.Path > LastPath || RepeatsAnId(request.LookIds))
        {
            RefuseCreate(session, CreateResults.General);
            return;
        }

        if (!CharacterNames.IsAllowed(request.Name))
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
        if (LoadedFor(delete.AccountId, session)?.Exists(c => c.Id == delete.CharacterId) != true)
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

        if (!CharacterNames.IsAllowed(rename.NewName))
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

        // Stage 1 has no world to enter, so a character that could be selected answers the general reason too.
        byte reason = target switch
        {
            null => SelectReasons.General,
            { RequiresRename: true } => SelectReasons.MustRename,
            { Disabled: true } => SelectReasons.Disabled,
            _ => SelectReasons.General,
        };
        select.Session.Send(new CharacterSelectFailed { Reason = reason });
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
                FactionId = summary.FactionId,
                Level = summary.Level,
                WorldId = summary.WorldId,
                ZoneId = summary.ZoneId,
                Position = summary.Position,
                PathId = summary.Path,
                Disabled = summary.Disabled,
                RequiresRename = summary.RequiresRename,
            };
            character.Bones.AddRange(summary.Bones);
            list.Characters.Add(character);
        }

        return list;
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
