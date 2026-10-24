using Microsoft.Extensions.Logging;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Maps.Movement;

// Each player's movement, relayed to the players who see it (decisions.md, design checkpoint item 8). A batch from a player with control
// waits on the player until the next replication flush, which sends one UnitMovement per batch, in arrival order and never
// conflated: a blended key appends to the running track, so dropping one loses keys. The client clamps a key older than its newest,
// so each player's relayed time never runs backwards, and a viewer's fit is restarted (Reset) on the first relay after its create and
// on every viewer after a grant or a lost batch. Each batch is serialised at most twice, with and without Reset. Touched only on the
// map's tick; relaying allocates nothing once its buffers are warm.
internal sealed class MovementRelay
{
    // Batches a player may queue between flushes; the client sends one per interval, so 16 is several intervals' worth.
    public const int PendingLimit = 16;

    // UnitMovement counts its commands in 5 bits.
    private const int CommandLimit = 31;

    private const byte VelocityArm = 8;
    private const byte MoveArm = 11;
    private const byte StateArm = 24;
    private const byte ModeArm = 27;

    private static readonly KeyValuePair<string, object?>[] s_reasons =
    [
        new("reason", "older"), new("reason", "too_long"), new("reason", "ring_full"), new("reason", "refused"),
    ];

    private readonly IMessageSerializer _serializer;
    private readonly MapMetrics _metrics;
    private readonly ILogger _logger;
    private readonly ushort _worldId;
    private readonly KeyValuePair<string, object?> _worldTag;
    private readonly UnitMovement _movement = new();
    private readonly long[] _dropped = new long[s_reasons.Length];
    private long _relayed;

    public MovementRelay(IMessageSerializer serializer, MapMetrics metrics, ILogger logger, ushort worldId, KeyValuePair<string, object?> worldTag)
    {
        _serializer = serializer;
        _metrics = metrics;
        _logger = logger;
        _worldId = worldId;
        _worldTag = worldTag;
    }

    public long Relayed => Volatile.Read(ref _relayed);

    public long Dropped(RelayDrop reason) => Volatile.Read(ref _dropped[(int)reason]);

    // A batch past the limit is lost, so the relay after the pending batches resets every viewer's fit rather than play on across the
    // gap; the pending batches came before it and relay as they would have.
    public void Queue(Player mover, RelayBatch batch, long nowMs)
    {
        if (mover.PendingCount == PendingLimit)
        {
            Drop(RelayDrop.RingFull);
            mover.LostAfterPending = true;
            return;
        }

        mover.Pending[mover.PendingCount] = batch;
        mover.PendingAtMs[mover.PendingCount] = nowMs;
        mover.PendingCount++;
    }

    // The mover's batches since the last flush came before a correction placed the unit, so relaying them would step it back.
    public static void Discard(Player mover)
    {
        Array.Clear(mover.Pending, 0, mover.PendingCount);
        mover.PendingCount = 0;
        mover.LostAfterPending = false;
    }

    public void Flush(Dictionary<ulong, Player>.ValueCollection players)
    {
        foreach (Player mover in players)
        {
            for (int i = 0; i < mover.PendingCount; i++)
            {
                RelayBatch batch = mover.Pending[i]!;
                mover.Pending[i] = null;
                Relay(mover, batch, mover.PendingAtMs[i]);
            }

            mover.PendingCount = 0;
            if (mover.LostAfterPending)
            {
                mover.LostAfterPending = false;
                mover.ResetNext = true;
            }
        }
    }

    private void Relay(Player mover, RelayBatch batch, long arrivedAtMs)
    {
        if (batch.Commands.Length > CommandLimit)
        {
            Drop(RelayDrop.TooLong);
            mover.ResetNext = true;
            return;
        }

        if (!mover.ResetNext && mover.HasRelayed && batch.Time < mover.LastBatchTime)
        {
            Drop(RelayDrop.Older);
            return;
        }

        if (mover.Viewers.Count > 0 && !Send(mover, batch))
        {
            Drop(RelayDrop.Refused);
            mover.ResetNext = true;
            return;
        }

        mover.ResetNext = false;
        mover.HasRelayed = true;
        mover.LastBatchTime = batch.Time;
        mover.LastBatchAtMs = arrivedAtMs;
        KeepMotion(mover, batch);
        Interlocked.Increment(ref _relayed);
        _metrics.BatchRelayed(_worldTag);
    }

    // A refusal comes from the first serialisation, before any viewer is sent anything: the two forms differ only in Reset, which is
    // marked. It is a declaration defect: it is logged and counted, the next relay resets, and no session is ended over it.
    private bool Send(Player mover, RelayBatch batch)
    {
        _movement.UnitId = mover.UnitId;
        _movement.Time = batch.Time;
        _movement.ServerMoved = false;
        _movement.Commands.Clear();
        _movement.Commands.AddRange(batch.Commands);
        SerializedMessage? plain = null;
        SerializedMessage? reset = null;
        try
        {
            foreach (Player viewer in mover.Viewers)
            {
                bool viewerReset = viewer.Visible.TryGetValue(mover, out bool fresh) && fresh;
                SerializedMessage message;
                if (mover.ResetNext || viewerReset)
                    message = reset ??= Serialize(reset: true);
                else
                    message = plain ??= Serialize(reset: false);

                viewer.Session.Send(message);
                if (viewerReset)
                    viewer.Visible[mover] = false;
            }

            return true;
        }
        catch (Exception exception) when (exception is InvalidOperationException or CodecException)
        {
            MapLog.RelayRefused(_logger, _worldId, mover.Character.Id, exception);
            return false;
        }
        finally
        {
            plain?.Release();
            reset?.Release();
            _movement.Commands.Clear();
        }
    }

    private SerializedMessage Serialize(bool reset)
    {
        _movement.Reset = reset;
        return _serializer.Serialize(_movement);
    }

    private static void KeepMotion(Player mover, RelayBatch batch)
    {
        foreach (Server0262Part5 command in batch.Commands)
        {
            switch (command.Unknown0)
            {
                case VelocityArm:
                    mover.LatestVelocity = command;
                    break;
                case MoveArm:
                    mover.LatestMove = command;
                    break;
                case StateArm:
                    mover.LatestState = command;
                    break;
                case ModeArm:
                    mover.LatestMode = command;
                    break;
            }
        }
    }

    private void Drop(RelayDrop reason)
    {
        Interlocked.Increment(ref _dropped[(int)reason]);
        _metrics.BatchDropped(_worldTag, s_reasons[(int)reason]);
    }
}
