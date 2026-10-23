using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Testing;

// Hosts the maps a test adds, one per world; a world with none is not hosted.
public sealed class FakeMapDirectory : IMapDirectory
{
    private readonly Lock _gate = new();
    private readonly Dictionary<ushort, FakeMapInbox> _maps = [];

    public FakeMapInbox Add(ushort worldId)
    {
        var map = new FakeMapInbox(worldId);
        lock (_gate)
            _maps[worldId] = map;

        return map;
    }

    public IMapInbox? Host(ushort worldId)
    {
        lock (_gate)
            return _maps.GetValueOrDefault(worldId);
    }
}

// Records what the realm posts to a map, in order.
public sealed class FakeMapInbox : IMapInbox
{
    private readonly Lock _gate = new();
    private readonly List<MapMessage> _posted = [];
    private int _highPosts;

    public FakeMapInbox(ushort worldId)
    {
        WorldId = worldId;
    }

    public ushort WorldId { get; }

    public IReadOnlyList<MapMessage> Posted
    {
        get
        {
            lock (_gate)
                return [.. _posted];
        }
    }

    // Counted before the message is recorded, so a test that sees the message sees the count.
    public int HighPosts => Volatile.Read(ref _highPosts);

    // False refuses every post, as a stopped map does.
    public bool Accepting { get; set; } = true;

    // True makes both posts throw before recording anything: a fault in the poster's handler.
    public bool Throwing { get; set; }

    public bool Post(MapMessage message)
    {
        if (Throwing)
            throw new InvalidOperationException("Post refused by the test.");

        if (!Accepting)
            return false;

        lock (_gate)
            _posted.Add(message);

        return true;
    }

    public bool PostHigh(MapMessage message)
    {
        if (Throwing)
            throw new InvalidOperationException("PostHigh refused by the test.");

        if (!Accepting)
            return false;

        Interlocked.Increment(ref _highPosts);
        lock (_gate)
            _posted.Add(message);

        return true;
    }
}
