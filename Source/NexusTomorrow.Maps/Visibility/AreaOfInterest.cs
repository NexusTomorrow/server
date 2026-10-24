using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Owners;

namespace NexusTomorrow.Maps.Visibility;

// Which players each player sees (decisions.md R5, the design checkpoint's item 7). At each replication flush, a viewer keeps a seen
// player while it stays within the drop radius and the predicate allows it, and is sent a create for an unseen one within the admit
// radius, nearest first, up to the cap and the admits a check allows. A create, its path and a destroy are serialised once per target
// per check, whatever the number of viewers. Only players in the world see or are seen. Touched only on the map's tick; a steady
// flush, with nobody admitted or dropped, allocates nothing.
internal sealed class AreaOfInterest
{
    private static readonly NearestFirst s_nearestFirst = new();

    private readonly MapSettings _settings;
    private readonly IMessageSerializer _serializer;
    private readonly IVisibilityPredicate _visibility;
    private readonly MapMetrics _metrics;
    private readonly ILogger _logger;
    private readonly ushort _worldId;
    private readonly KeyValuePair<string, object?> _worldTag;
    private readonly MapGrid _grid;
    private readonly float _admitSquared;
    private readonly float _dropSquared;
    private readonly List<Player> _near = [];
    private readonly List<Candidate> _candidates = [];
    private readonly List<Player> _drops = [];
    private readonly Dictionary<Player, Serialized> _serialized = new(ReferenceEqualityComparer.Instance);
    private readonly UnitDestroyed _destroy = new();

    public AreaOfInterest(
        MapSettings settings, IMessageSerializer serializer, IVisibilityPredicate visibility, MapMetrics metrics, ILogger logger, ushort worldId,
        KeyValuePair<string, object?> worldTag)
    {
        _settings = settings;
        _serializer = serializer;
        _visibility = visibility;
        _metrics = metrics;
        _logger = logger;
        _worldId = worldId;
        _worldTag = worldTag;
        _grid = new MapGrid(settings.DropRadius);
        _admitSquared = (float)settings.AdmitRadius * settings.AdmitRadius;
        _dropSquared = (float)settings.DropRadius * settings.DropRadius;
    }

    public void Check(Dictionary<ulong, Player>.ValueCollection players, long nowMs)
    {
        try
        {
            foreach (Player player in players)
            {
                if (!player.InWorld)
                    continue;

                if (player.InGrid)
                    _grid.Update(player);
                else
                    _grid.Add(player);
            }

            foreach (Player viewer in players)
            {
                if (viewer.InWorld)
                    Check(viewer, nowMs);
            }
        }
        finally
        {
            ReleaseSerialized();
        }
    }

    // Takes a leaving player out of every set and off the grid. With sendDestroys, each player who saw it is sent its destroy.
    public void Forget(Player leaving, bool sendDestroys)
    {
        if (sendDestroys && leaving.Viewers.Count > 0)
        {
            _destroy.UnitId = leaving.UnitId;
            SerializedMessage destroy = _serializer.Serialize(_destroy);
            try
            {
                foreach (Player viewer in leaving.Viewers)
                {
                    viewer.Visible.Remove(leaving);
                    viewer.Session.Send(destroy);
                    _metrics.UnitDropped(_worldTag);
                }
            }
            finally
            {
                destroy.Release();
            }
        }
        else
        {
            foreach (Player viewer in leaving.Viewers)
                viewer.Visible.Remove(leaving);
        }

        foreach (Player target in leaving.Visible.Keys)
            target.Viewers.Remove(leaving);

        leaving.Visible.Clear();
        leaving.Viewers.Clear();
        _grid.Remove(leaving);
    }

    private void Check(Player viewer, long nowMs)
    {
        _drops.Clear();
        foreach (Player target in viewer.Visible.Keys)
        {
            if (!Keeps(viewer, target))
                _drops.Add(target);
        }

        foreach (Player target in _drops)
            Drop(viewer, target);

        int room = Math.Min(_settings.VisibleCap - viewer.Visible.Count, _settings.SwapsPerCheck);
        if (room <= 0)
            return;

        _near.Clear();
        _grid.Near(viewer, _near);
        _candidates.Clear();
        foreach (Player target in _near)
        {
            if (ReferenceEquals(target, viewer) || !target.InWorld || viewer.Visible.ContainsKey(target))
                continue;

            float distanceSquared = Vector3.DistanceSquared(viewer.Location.Position, target.Location.Position);
            if (distanceSquared <= _admitSquared && _visibility.CanSee(viewer.Character.Id, target.Character.Id))
                _candidates.Add(new Candidate(target, distanceSquared));
        }

        _candidates.Sort(s_nearestFirst);
        for (int i = 0; i < _candidates.Count && room > 0; i++)
        {
            if (Admit(viewer, _candidates[i].Target, nowMs))
                room--;
        }
    }

    private bool Keeps(Player viewer, Player target) =>
        target.InWorld
        && Vector3.DistanceSquared(viewer.Location.Position, target.Location.Position) <= _dropSquared
        && _visibility.CanSee(viewer.Character.Id, target.Character.Id);

    // The viewer is sent the target's create, then its path, and the pair is marked so the target's next relay to this viewer resets.
    private bool Admit(Player viewer, Player target, long nowMs)
    {
        ref Serialized serialized = ref CollectionsMarshal.GetValueRefOrAddDefault(_serialized, target, out bool known);
        if (!known)
            serialized = SerializeArrival(target, nowMs);
        if (serialized.Create is null || serialized.Path is null)
            return false;

        viewer.Session.Send(serialized.Create);
        viewer.Session.Send(serialized.Path);
        viewer.Visible[target] = true;
        target.Viewers.Add(viewer);
        _metrics.UnitAdmitted(_worldTag);
        return true;
    }

    private void Drop(Player viewer, Player target)
    {
        viewer.Visible.Remove(target);
        target.Viewers.Remove(viewer);
        ref Serialized serialized = ref CollectionsMarshal.GetValueRefOrAddDefault(_serialized, target, out _);
        if (serialized.Destroy is null)
        {
            _destroy.UnitId = target.UnitId;
            serialized.Destroy = _serializer.Serialize(_destroy);
        }

        viewer.Session.Send(serialized.Destroy);
        _metrics.UnitDropped(_worldTag);
    }

    // A create the send gate or the codec refuses is a declaration defect: it is logged once per check and the target is created for
    // no viewer, so no session is ended over it.
    private Serialized SerializeArrival(Player target, long nowMs)
    {
        SerializedMessage? create = null;
        try
        {
            create = _serializer.Serialize(WorldEntry.RemoteCreate(target, _settings.RealmId, SenderTime(target, nowMs)));
            SerializedMessage path = _serializer.Serialize(WorldEntry.PathOf(target));
            return new Serialized(create, path, null);
        }
        catch (Exception exception) when (exception is InvalidOperationException or CodecException)
        {
            create?.Release();
            MapLog.ArrivalRefused(_logger, _worldId, target.Character.Id, exception);
            return default;
        }
    }

    // The target's clock as the viewer's fit will first see it: its last relayed time moved on by the time since that batch arrived.
    private static uint SenderTime(Player target, long nowMs) =>
        target.HasRelayed ? unchecked(target.LastBatchTime + (uint)(nowMs - target.LastBatchAtMs)) : 0;

    private void ReleaseSerialized()
    {
        foreach (Serialized serialized in _serialized.Values)
        {
            serialized.Create?.Release();
            serialized.Path?.Release();
            serialized.Destroy?.Release();
        }

        _serialized.Clear();
    }

    private readonly record struct Candidate(Player Target, float DistanceSquared);

    private record struct Serialized(SerializedMessage? Create, SerializedMessage? Path, SerializedMessage? Destroy);

    private sealed class NearestFirst : IComparer<Candidate>
    {
        public int Compare(Candidate x, Candidate y) => x.DistanceSquared.CompareTo(y.DistanceSquared);
    }
}
