using NexusTomorrow.Maps.Movement;
using NexusTomorrow.Maps.Timing;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Maps;

// A character a map holds. Touched only on the map's tick.
internal sealed class Player
{
    public Player(IWorldSession session, long accountId, CharacterRecord character, uint unitId, long serial, IRealmInbox returnTo)
    {
        Session = session;
        AccountId = accountId;
        Character = character;
        UnitId = unitId;
        Serial = serial;
        ReturnTo = returnTo;
        Location = character.Location;
    }

    public IWorldSession Session { get; }

    public long AccountId { get; }

    public CharacterRecord Character { get; }

    public uint UnitId { get; }

    // Unique to the arrival on this map.
    public long Serial { get; }

    public IRealmInbox ReturnTo { get; }

    public CharacterLocation Location { get; private set; }

    // Bumped by every change of location; the version a save holds is the one it was queued at.
    public uint Version { get; private set; }

    public uint SavedVersion { get; private set; }

    public bool IsDirty => Version != SavedVersion;

    public TimerHandle SaveTimer { get; set; }

    // The ticket of the movement control grant the world entry sent.
    public uint ControlTicket { get; set; }

    // True once the client's receipt of ControlTicket arrived: the client moves the unit.
    public bool HasControl { get; set; }

    // True once WorldEntryComplete went out.
    public bool InWorld { get; set; }

    // True while a logout request waits for its confirm.
    public bool LogoutPending { get; set; }

    // The players this player sees, each with whether its next relay to this player resets (true from the create on).
    public Dictionary<Player, bool> Visible { get; } = new(ReferenceEqualityComparer.Instance);

    // The players who see this one; the mirror of their Visible, kept with it.
    public HashSet<Player> Viewers { get; } = new(ReferenceEqualityComparer.Instance);

    // The grid cell the player was last filed under, while InGrid.
    public (int X, int Z) Cell { get; set; }

    public bool InGrid { get; set; }

    // The time of the last batch relayed for this player, on its own clock, and the map's time when that batch arrived.
    public bool HasRelayed { get; set; }

    public uint LastBatchTime { get; set; }

    public long LastBatchAtMs { get; set; }

    // The movement batches that arrived since the last flush, in order, each with the map's time it arrived at; at most the arrays'
    // length.
    public RelayBatch?[] Pending { get; } = new RelayBatch?[MovementRelay.PendingLimit];

    public long[] PendingAtMs { get; } = new long[MovementRelay.PendingLimit];

    public int PendingCount { get; set; }

    // True when this player's next relay resets for every viewer: after a control grant, a lost batch or a refused relay.
    public bool ResetNext { get; set; }

    // True when a batch was lost to a full queue: the gap follows every batch still pending, so the reset waits until they are relayed.
    public bool LostAfterPending { get; set; }

    // The latest velocity, commanded velocity, state and mode commands relayed for this player, which a create for a new viewer
    // carries so the unit moves as it does for everyone else.
    public Server0262Part5? LatestVelocity { get; set; }

    public Server0262Part5? LatestMove { get; set; }

    public Server0262Part5? LatestState { get; set; }

    public Server0262Part5? LatestMode { get; set; }

    public void MoveTo(CharacterLocation location)
    {
        if (Location.Equals(location))
            return;

        Location = location;
        Version++;
    }

    public void Committed(uint version)
    {
        if (version > SavedVersion)
            SavedVersion = version;
    }
}
