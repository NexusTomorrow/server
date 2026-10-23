using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Testing;

// Records what a map posts to the realm, in order.
public sealed class FakeRealmInbox : IRealmInbox
{
    private readonly Lock _gate = new();
    private readonly List<RealmMessage> _posted = [];

    public IReadOnlyList<RealmMessage> Posted
    {
        get
        {
            lock (_gate)
                return [.. _posted];
        }
    }

    // Runs before the post is recorded, so a test can read what else the poster had done by then.
    public Action<RealmMessage>? OnPost { get; set; }

    public bool Post(RealmMessage message)
    {
        OnPost?.Invoke(message);
        lock (_gate)
            _posted.Add(message);

        return true;
    }
}
