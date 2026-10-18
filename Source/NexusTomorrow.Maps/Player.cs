using NexusTomorrow.Maps.Timing;
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
