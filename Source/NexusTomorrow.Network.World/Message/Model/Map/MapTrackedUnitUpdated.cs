using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Places a tracked unit on the map, or moves it there; the client raises MapTrackedUnitUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0849, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MapTrackedUnitUpdated
{
    /// <summary>
    /// The tracked unit, as a TrackingSlot id.
    /// </summary>
    [UInt(32)] public uint TrackingSlotId { get; set; }

    /// <summary>
    /// The unit's position, where the map shows it.
    /// </summary>
    [Float32] public Vector3 Position { get; set; }

    /// <summary>
    /// The zone the map places the unit in.
    /// </summary>
    [UInt(15)] public uint ZoneId { get; set; }
}
