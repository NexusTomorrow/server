using System.Diagnostics;
using System.Numerics;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Maps.Timing;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Maps;

/// <summary>
/// One world's map: it holds the characters the realm hands it, saves a changed one on a jittered timer, and hands each back on
/// leaving or stopping. A tick runs in this order: drain the high queue in full, drain the normal queue to its floor and budget,
/// run the timers, hand the due saves to the write-behind queue, then stop when a stop was requested. Only <see cref="Tick"/>
/// touches the map's state, so the state needs no locks. An arriving character gets the world entry (<c>WorldEntry</c>); the client's
/// <c>WorldReady</c> is answered once with <c>WorldEntryComplete</c>.
/// A logout request is answered with a countdown of 0; its confirm hands the character back and returns the client to character select.
/// </summary>
public sealed class MapOwner : IMapInbox, IOwner, ITickable
{
    private readonly ushort _worldId;
    private readonly MapSettings _settings;
    private readonly IWriteBehind _persistence;
    private readonly Random _random;
    private readonly MapMetrics _metrics;
    private readonly ILogger<MapOwner> _logger;
    private readonly KeyValuePair<string, object?> _worldTag;
    private readonly long _saveIntervalMs;
    private readonly long _drainBudgetTimestamps;
    private readonly Channel<MapMessage> _high = Channel.CreateUnbounded<MapMessage>(new UnboundedChannelOptions { SingleReader = true });
    private readonly Channel<MapMessage> _normal = Channel.CreateUnbounded<MapMessage>(new UnboundedChannelOptions { SingleReader = true });
    private readonly TaskCompletionSource _stopped = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly TimerWheel<MapTimer> _wheel = new(0);
    private readonly SaveTimerHandler _timers;
    private readonly Dictionary<IWorldSession, Player> _bySession = new(ReferenceEqualityComparer.Instance);
    private readonly Dictionary<ulong, Player> _byCharacter = [];
    private readonly List<Player> _due = [];
    private readonly WorldEntryComplete _entryComplete = new();

    // LogoutCountdown.Duration 0 puts the client's deadline at now, so it confirms at once. Unknown1 is unmarked, so the send gate
    // refuses it set.
    private readonly LogoutCountdown _logoutCountdown = new();

    // With a reason other than 6, 7, 8, 12, 15, 16, 19 and 20, ToCharacterSelect decides (WorldLogout.Unknown1); 0 is not one of them.
    private readonly WorldLogout _logoutToCharacterSelect = new() { ToCharacterSelect = true, Unknown1 = 0 };

    // Written by any thread: the queue count by posters and the tick, the stop request by StopAsync.
    private int _queued;
    private int _stopRequested;

    // Written by the tick, read from any thread.
    private int _playerCount;
    private long _ticks;
    private long _lastTickAllocatedBytes;
    private long _droppedMessages;

    // Touched only on the tick.
    private long _nowMs;
    private uint _lastUnitId;
    private uint _lastControlTicket;
    private long _lastSerial;
    private bool _finished;

    /// <summary>
    /// Create the map for world <paramref name="worldId"/>. <paramref name="random"/> is read only on the tick, so a generator
    /// shared with other maps must be safe to use from their threads.
    /// </summary>
    public MapOwner(
        ushort worldId, MapSettings settings, IWriteBehind persistence, Random random, MapMetrics metrics, ILogger<MapOwner> logger)
    {
        _worldId = worldId;
        _settings = settings;
        _persistence = persistence;
        _random = random;
        _metrics = metrics;
        _logger = logger;
        _worldTag = new KeyValuePair<string, object?>("world", (int)worldId);
        _saveIntervalMs = (long)settings.SaveInterval.TotalMilliseconds;
        _drainBudgetTimestamps = (long)(settings.NormalDrainBudget.TotalSeconds * Stopwatch.Frequency);
        _timers = new SaveTimerHandler(this);
        Name = $"map {worldId}";
    }

    /// <inheritdoc/>
    public ushort WorldId => _worldId;

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int TickMs => _settings.TickMs;

    /// <inheritdoc/>
    public int QueueLength => Volatile.Read(ref _queued);

    /// <inheritdoc/>
    public long Handled => Volatile.Read(ref _ticks);

    /// <summary>
    /// How many characters the map holds.
    /// </summary>
    public int PlayerCount => Volatile.Read(ref _playerCount);

    /// <summary>
    /// The bytes the last tick allocated on its thread, measured with <see cref="GC.GetAllocatedBytesForCurrentThread"/>.
    /// </summary>
    public long LastTickAllocatedBytes => Volatile.Read(ref _lastTickAllocatedBytes);

    /// <summary>
    /// How many client messages the map read but did not act on.
    /// </summary>
    public long DroppedMessages => Volatile.Read(ref _droppedMessages);

    /// <inheritdoc/>
    public bool Post(MapMessage message) => Write(_normal.Writer, message);

    /// <inheritdoc/>
    public bool PostHigh(MapMessage message) => Write(_high.Writer, message);

    /// <summary>
    /// Ask the map to stop at its next tick: it refuses further messages, handles those already queued, hands every character
    /// back and disconnects its sessions. The task completes when that tick has run, so a map that is never ticked never completes.
    /// </summary>
    public Task StopAsync()
    {
        Volatile.Write(ref _stopRequested, 1);
        return _stopped.Task;
    }

    /// <inheritdoc/>
    public void Tick(long nowMs)
    {
        if (_finished)
            return;

        long allocatedAtStart = GC.GetAllocatedBytesForCurrentThread();
        long startTimestamp = Stopwatch.GetTimestamp();
        _nowMs = nowMs;

        // The scheduler's clock counts from its own start and a map can open long after it, while the wheel starts at 0 and steps
        // one millisecond at a time while it holds a timer: an empty wheel is brought to just before now, so the timers the drain
        // schedules count from now and the timer run below walks one millisecond.
        if (_wheel.Count == 0)
            _wheel.Advance(nowMs - 1, _timers);

        // Completing the writers before the drain means nothing posted after this tick's drain goes unhandled, and later posts
        // return false.
        bool stopping = Volatile.Read(ref _stopRequested) != 0;
        if (stopping)
        {
            _high.Writer.TryComplete();
            _normal.Writer.TryComplete();
        }

        while (_high.Reader.TryRead(out MapMessage? urgent))
            Handle(urgent);

        int handled = 0;
        while (_normal.Reader.TryRead(out MapMessage? message))
        {
            Handle(message);
            handled++;
            if (!stopping && handled >= _settings.NormalDrainFloor && Stopwatch.GetTimestamp() - startTimestamp >= _drainBudgetTimestamps)
                break;
        }

        try
        {
            _wheel.Advance(nowMs, _timers);
        }
        catch (Exception exception)
        {
            TimersFailed(exception);
        }

        try
        {
            for (int i = 0; i < _due.Count; i++)
            {
                Player player = _due[i];
                if (player.IsDirty && _byCharacter.TryGetValue(player.Character.Id, out Player? held) && ReferenceEquals(held, player))
                    Save(player);
            }
        }
        catch (Exception exception)
        {
            TimersFailed(exception);
        }
        finally
        {
            _due.Clear();
        }

        if (stopping)
            Stop();

        Volatile.Write(ref _ticks, _ticks + 1);
        long allocated = GC.GetAllocatedBytesForCurrentThread() - allocatedAtStart;
        Volatile.Write(ref _lastTickAllocatedBytes, allocated);
        _metrics.TickCompleted(Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds, allocated, _worldTag);
    }

    internal Player? PlayerOf(IWorldSession session) => _bySession.TryGetValue(session, out Player? player) ? player : null;

    private bool Write(ChannelWriter<MapMessage> writer, MapMessage message)
    {
        // Counted before the write, so the tick's decrement can never run first.
        Interlocked.Increment(ref _queued);
        if (writer.TryWrite(message))
            return true;

        Interlocked.Decrement(ref _queued);
        return false;
    }

    private void Handle(MapMessage message)
    {
        Interlocked.Decrement(ref _queued);
        try
        {
            switch (message)
            {
                case CharacterArrived arrived:
                    Arrive(arrived);
                    break;
                case ClientLeft left:
                    if (_bySession.TryGetValue(left.Session, out Player? leaving))
                        HandBack(leaving);
                    break;
                case ClientWorldReady ready:
                    if (_bySession.TryGetValue(ready.Session, out Player? loaded) && !loaded.InWorld)
                        EnterWorldComplete(loaded, ready.ZoneId);
                    break;
                case ClientControlReceipt receipt:
                    if (_bySession.TryGetValue(receipt.Session, out Player? granted) && receipt.Ticket == granted.ControlTicket)
                        granted.HasControl = true;
                    break;
                case ClientMoved moved:
                    // Movement counts once the client has taken control on the grant's ticket.
                    if (_bySession.TryGetValue(moved.Session, out Player? mover) && mover.HasControl)
                        KeepMovement(mover, moved.Sample);
                    break;
                case ClientZoneChanged zoneChanged:
                    if (_bySession.TryGetValue(zoneChanged.Session, out Player? moving))
                        KeepZone(moving, zoneChanged.ZoneId);
                    break;
                case ClientLogoutRequested logoutRequest:
                    if (_bySession.TryGetValue(logoutRequest.Session, out Player? requester))
                        RequestLogout(requester, logoutRequest.Begin);
                    break;
                case ClientLogoutConfirmed logoutConfirmed:
                    if (_bySession.TryGetValue(logoutConfirmed.Session, out Player? confirmer) && confirmer.LogoutPending)
                        ConfirmLogout(confirmer);
                    else
                        Drop(logoutConfirmed.Session, nameof(ClientLogoutConfirmed), "no logout request pending");
                    break;
                case CharacterSaved saved:
                    if (_byCharacter.TryGetValue(saved.CharacterId, out Player? savedPlayer) && savedPlayer.Serial == saved.Serial)
                        savedPlayer.Committed(saved.Version);
                    break;
            }
        }
        catch (Exception exception)
        {
            Fault(message, exception);
        }
    }

    private void Arrive(CharacterArrived arrived)
    {
        IWorldSession session = arrived.Session;
        CharacterRecord character = arrived.Character;

        // The realm hands over only characters at rest (P6), so a character already held here is unreachable; the held copy stays
        // authoritative, which is why no return is posted for this one.
        if (_byCharacter.ContainsKey(character.Id))
        {
            MapLog.AlreadyHeld(_logger, _worldId, character.Id, session.Id);
            session.Disconnect("the character is already in the world");
            return;
        }

        if (_bySession.ContainsKey(session) || !session.TryEnterMap(this))
        {
            arrived.ReturnTo.Post(new CharacterReturned(arrived.AccountId, character.Id, character.Location));
            return;
        }

        var player = new Player(session, arrived.AccountId, character, ++_lastUnitId, ++_lastSerial, arrived.ReturnTo);
        _bySession.Add(session, player);
        _byCharacter.Add(character.Id, player);
        Volatile.Write(ref _playerCount, _byCharacter.Count);
        player.SaveTimer = _wheel.Schedule(
            _nowMs + _random.NextInt64(1, _saveIntervalMs + 1), new MapTimer(MapTimerKind.Save, character.Id));
        MapLog.Entered(_logger, _worldId, character.Id, player.UnitId);
        player.ControlTicket = ++_lastControlTicket;
        WorldEntry.Send(player, _settings);
    }

    private void EnterWorldComplete(Player player, ushort zoneId)
    {
        KeepZone(player, zoneId);
        player.InWorld = true;
        player.Session.Send(_entryComplete);
    }

    private void RequestLogout(Player player, bool begin)
    {
        player.LogoutPending = begin;
        if (begin)
            player.Session.Send(_logoutCountdown);
    }

    // The hand-back queues the save when the location changed (HandBack's IsDirty check) and posts the return before WorldLogout goes
    // out, so the client's next select finds the character at rest. The connection stays open: a client that chose to exit quits
    // itself, and its closed connection no longer routes here.
    private void ConfirmLogout(Player player)
    {
        IWorldSession session = player.Session;
        HandBack(player);
        session.Send(_logoutToCharacterSelect);
    }

    private void Drop(IWorldSession session, string message, string reason)
    {
        Interlocked.Increment(ref _droppedMessages);
        _metrics.Dropped(_worldTag);
        MapLog.MessageDropped(_logger, _worldId, message, session.Id, reason);
    }

    // A zone of 0 is not a zone: the client's report on taking control can carry it, and the player keeps the zone it stands in.
    private static void KeepZone(Player player, ushort zoneId)
    {
        if (zoneId != 0)
            player.MoveTo(player.Location with { ZoneId = zoneId });
    }

    private static void KeepMovement(Player player, MovementSample sample)
    {
        CharacterLocation location = player.Location;
        if (sample.Position is Vector3 position)
            location = location with { Position = position };
        if (sample.Facing is float facing)
            location = location with { Facing = facing };

        player.MoveTo(location);
    }

    // The save is queued before the realm hears of the return, so a select's load queued after the return under the same key reads
    // it (decisions.md D9 2a, 2c). LeaveMap comes last: a session that throws cannot keep the character from the realm.
    private void HandBack(Player player)
    {
        ulong characterId = player.Character.Id;
        _bySession.Remove(player.Session);
        _byCharacter.Remove(characterId);
        Volatile.Write(ref _playerCount, _byCharacter.Count);
        _wheel.Cancel(player.SaveTimer);
        if (player.IsDirty)
            Save(player);

        player.ReturnTo.Post(new CharacterReturned(player.AccountId, characterId, player.Location));
        MapLog.HandedBack(_logger, _worldId, characterId);
        player.Session.LeaveMap(this);
    }

    // The completion captures the id, the arrival's serial and the version, never the player: a save from an earlier stay, or one
    // that did not commit, cannot mark this one clean.
    private void Save(Player player)
    {
        ulong characterId = player.Character.Id;
        long serial = player.Serial;
        uint version = player.Version;
        _persistence.Enqueue(new SaveCharacterLocationWork(
            characterId,
            player.Location,
            result =>
            {
                if (result.Outcome == WorkOutcome.Committed)
                    Post(new CharacterSaved(characterId, serial, version));
            }));
    }

    private void OnSaveTimer(ulong characterId, long dueMs)
    {
        if (!_byCharacter.TryGetValue(characterId, out Player? player))
            return;

        player.SaveTimer = _wheel.Schedule(dueMs + _saveIntervalMs, new MapTimer(MapTimerKind.Save, characterId));
        _due.Add(player);
    }

    private void Stop()
    {
        Player[] held = [.. _byCharacter.Values];
        foreach (Player player in held)
        {
            // The stop goes on for every other character, and HandBack calls the session last, so a session that throws has had
            // its character returned.
            try
            {
                HandBack(player);
            }
            catch (Exception exception)
            {
                MapLog.HandlerFailed(_logger, _worldId, "Stop", player.Session.Id, exception);
                _metrics.Fault(_worldTag);
            }

            try
            {
                player.Session.Disconnect("the map stopped");
            }
            catch (Exception exception)
            {
                MapLog.HandlerFailed(_logger, _worldId, "Stop", player.Session.Id, exception);
                _metrics.Fault(_worldTag);
            }
        }

        _finished = true;
        _stopped.TrySetResult();
    }

    // The failure policy (spec section 1): log the message, disconnect its session, and keep ticking. One message posts at most one
    // return. For an arrival: when no session holds its character, the character goes back to the realm as it arrived; when the
    // arrival's own session holds it, that player goes back; when another session holds it, it stays. For any other session message,
    // the session's player goes back.
    private void Fault(MapMessage message, Exception exception)
    {
        var sessionMessage = message as SessionMessage;
        MapLog.HandlerFailed(_logger, _worldId, message.GetType().Name, sessionMessage?.Session.Id ?? 0, exception);
        _metrics.Fault(_worldTag);
        if (sessionMessage is null)
            return;

        try
        {
            sessionMessage.Session.Disconnect("the map failed handling a message");
        }
        catch (Exception disconnectFailure)
        {
            MapLog.HandlerFailed(_logger, _worldId, "Disconnect", sessionMessage.Session.Id, disconnectFailure);
        }

        try
        {
            if (message is CharacterArrived arrived)
            {
                // A character held by another session stays that session's; one held through this arrival goes back with it.
                if (!_byCharacter.TryGetValue(arrived.Character.Id, out Player? held))
                    arrived.ReturnTo.Post(new CharacterReturned(arrived.AccountId, arrived.Character.Id, arrived.Character.Location));
                else if (ReferenceEquals(held.Session, arrived.Session))
                    HandBack(held);
            }
            else if (_bySession.TryGetValue(sessionMessage.Session, out Player? player))
            {
                HandBack(player);
            }
        }
        catch (Exception handBackFailure)
        {
            MapLog.HandlerFailed(_logger, _worldId, "HandBack", sessionMessage.Session.Id, handBackFailure);
        }
    }

    private void TimersFailed(Exception exception)
    {
        MapLog.TimersFailed(_logger, _worldId, exception);
        _metrics.Fault(_worldTag);
    }

    private sealed class SaveTimerHandler : ITimerHandler<MapTimer>
    {
        private readonly MapOwner _owner;

        public SaveTimerHandler(MapOwner owner) => _owner = owner;

        public void OnTimer(in MapTimer payload, long dueMs) => _owner.OnSaveTimer(payload.CharacterId, dueMs);
    }
}
