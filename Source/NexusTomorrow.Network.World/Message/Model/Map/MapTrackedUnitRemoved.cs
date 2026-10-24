using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Removes a tracked unit from the map; the client raises MapTrackedUnitDisable.
/// </summary>
[WireMessage(GameMessageOpcode.Server0848, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MapTrackedUnitRemoved
{
    /// <summary>
    /// The tracked unit, by the TrackingSlot id <see cref="MapTrackedUnitUpdated"/> placed it under.
    /// </summary>
    [UInt(32)] public uint TrackingSlotId { get; set; }
}
