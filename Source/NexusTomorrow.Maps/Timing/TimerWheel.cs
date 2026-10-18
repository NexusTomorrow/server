namespace NexusTomorrow.Maps.Timing;

/// <summary>
/// Millisecond timers with value payloads on one owner's thread; not thread-safe.
/// </summary>
/// <typeparam name="TPayload">The value carried by each timer.</typeparam>
public sealed class TimerWheel<TPayload>
    where TPayload : struct
{
    private const int SlotCount = 1024;
    private const int SlotMask = SlotCount - 1;
    private const int None = -1;

    // Per slot, the first and last node due in that slot; None when the slot is empty.
    private readonly int[] _heads = new int[SlotCount];
    private readonly int[] _tails = new int[SlotCount];

    private Node[] _nodes;
    private int _used;
    private int _freeHead = None;
    private int _count;
    private long _lastMs;

    /// <summary>
    /// Create a wheel whose clock reads <paramref name="startMs"/>.
    /// </summary>
    /// <param name="startMs">The millisecond the wheel has already advanced to.</param>
    /// <param name="capacity">How many timers the wheel has room for before it grows.</param>
    public TimerWheel(long startMs, int capacity = 16)
    {
        _lastMs = startMs;
        _nodes = new Node[Math.Max(1, capacity)];
        Array.Fill(_heads, None);
        Array.Fill(_tails, None);
    }

    /// <summary>
    /// How many timers are scheduled.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// The last millisecond the wheel advanced to.
    /// </summary>
    public long LastMs => _lastMs;

    /// <summary>
    /// Schedule a timer; a time at or before <see cref="LastMs"/> fires on the next <see cref="Advance"/>.
    /// </summary>
    /// <param name="dueMs">The millisecond the timer is due at.</param>
    /// <param name="payload">The value handed to the handler.</param>
    /// <returns>The handle that cancels the timer.</returns>
    public TimerHandle Schedule(long dueMs, in TPayload payload)
    {
        long due = Math.Max(dueMs, _lastMs + 1);
        int index = Allocate();
        ref Node node = ref _nodes[index];
        node.Payload = payload;
        node.Due = due;
        node.Live = true;

        int slot = (int)(due & SlotMask);
        int tail = _tails[slot];
        node.Prev = tail;
        node.Next = None;
        if (tail == None)
            _heads[slot] = index;
        else
            _nodes[tail].Next = index;
        _tails[slot] = index;

        _count++;
        return new TimerHandle(index, node.Generation);
    }

    /// <summary>
    /// Cancel a timer that has neither fired nor been cancelled.
    /// </summary>
    /// <param name="handle">The handle <see cref="Schedule"/> returned.</param>
    /// <returns>True when the timer was cancelled.</returns>
    public bool Cancel(TimerHandle handle)
    {
        if (handle.IsNone || (uint)handle.Index >= (uint)_used)
            return false;

        ref Node node = ref _nodes[handle.Index];
        if (!node.Live || node.Generation != handle.Generation)
            return false;

        Free(handle.Index);
        return true;
    }

    /// <summary>
    /// Fire every timer due at or before <paramref name="nowMs"/>, earliest first.
    /// </summary>
    /// <param name="nowMs">The millisecond to advance to.</param>
    /// <param name="handler">Receives each fired timer.</param>
    /// <returns>How many timers fired.</returns>
    public int Advance(long nowMs, ITimerHandler<TPayload> handler)
    {
        int fired = 0;
        while (_lastMs < nowMs)
        {
            if (_count == 0)
            {
                _lastMs = nowMs;
                break;
            }

            long t = _lastMs + 1;
            _lastMs = t;
            int slot = (int)(t & SlotMask);

            // The handler may schedule or cancel in this slot, so every fire restarts from the head.
            int index = _heads[slot];
            while (index != None)
            {
                ref Node node = ref _nodes[index];
                if (node.Due > t)
                {
                    index = node.Next;
                    continue;
                }

                TPayload payload = node.Payload;
                long due = node.Due;
                Free(index);
                try
                {
                    handler.OnTimer(in payload, due);
                }
                catch
                {
                    _lastMs = t - 1;
                    throw;
                }

                fired++;
                index = _heads[slot];
            }
        }

        return fired;
    }

    private int Allocate()
    {
        int index;
        if (_freeHead != None)
        {
            index = _freeHead;
            _freeHead = _nodes[index].Next;
        }
        else
        {
            if (_used == _nodes.Length)
                Array.Resize(ref _nodes, _nodes.Length * 2);
            index = _used++;
        }

        ref Node node = ref _nodes[index];
        node.Generation = node.Generation == int.MaxValue ? 1 : node.Generation + 1;
        return index;
    }

    private void Free(int index)
    {
        ref Node node = ref _nodes[index];
        int slot = (int)(node.Due & SlotMask);
        if (node.Prev == None)
            _heads[slot] = node.Next;
        else
            _nodes[node.Prev].Next = node.Next;
        if (node.Next == None)
            _tails[slot] = node.Prev;
        else
            _nodes[node.Next].Prev = node.Prev;

        node.Payload = default;
        node.Live = false;
        node.Prev = None;
        node.Next = _freeHead;
        _freeHead = index;
        _count--;
    }

    private struct Node
    {
        public TPayload Payload;
        public long Due;
        public int Prev;
        public int Next;
        public int Generation;
        public bool Live;
    }
}
