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
