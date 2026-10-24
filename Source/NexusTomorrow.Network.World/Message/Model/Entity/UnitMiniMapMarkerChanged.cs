using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets a unit's minimap marker.
/// </summary>
/// <remarks>
/// The client keeps the marker where it keeps <see cref="UnitCreate.Unknown18"/> and raises UnitMiniMapMarkerChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server088A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitMiniMapMarkerChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's minimap marker, by id. Unit:GetMiniMapMarkers returns it, or else the minimap marker of the creature's
    /// TradeskillHarvestingInfo row for a tier the player knows.
    /// </summary>
    [UInt(14)] public ushort MiniMapMarkerId { get; set; }
}
