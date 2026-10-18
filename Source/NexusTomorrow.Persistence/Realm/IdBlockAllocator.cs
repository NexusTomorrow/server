namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Hands out ids from blocks taken off the database ahead of need. Used on one owner's thread only. It asks for a refill once fewer
/// than the refill mark remain, and not again until a block arrives.
/// </summary>
public sealed class IdBlockAllocator
{
    private readonly Queue<IdBlock> _blocks = new();
    private readonly int _refillBelow;
    private ulong _next;
    private int _leftInCurrent;
    private bool _refillRequested;

    /// <summary>
    /// Create an allocator that wants a refill below <paramref name="refillBelow"/> ids.
    /// </summary>
    public IdBlockAllocator(int refillBelow)
    {
        _refillBelow = refillBelow;
    }

    /// <summary>
    /// The ids left across every block held.
    /// </summary>
    public int Remaining { get; private set; }

    /// <summary>
    /// True when fewer than the refill mark remain and no refill is on its way.
    /// </summary>
    public bool WantsRefill => Remaining < _refillBelow && !_refillRequested;

    /// <summary>
    /// Record that a refill was asked for.
    /// </summary>
    public void MarkRefillRequested() => _refillRequested = true;

    /// <summary>
    /// Add a block, which also ends any wait for a refill.
    /// </summary>
    public void Add(IdBlock block)
    {
        _blocks.Enqueue(block);
        Remaining += block.Count;
        _refillRequested = false;
    }

    /// <summary>
    /// Take the next id; false when none is held.
    /// </summary>
    public bool TryTake(out ulong id)
    {
        if (_leftInCurrent == 0)
        {
            if (!_blocks.TryDequeue(out IdBlock block))
            {
                id = 0;
                return false;
            }

            _next = block.First;
            _leftInCurrent = block.Count;
        }

        id = _next++;
        _leftInCurrent--;
        Remaining--;
        return true;
    }
}
