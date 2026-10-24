using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets or removes one hex group on the zone map; the client raises ZoneMapUpdateHexGroup.
/// </summary>
[WireMessage(GameMessageOpcode.Server0965, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ZoneMapHexGroupUpdated
{
    /// <summary>
    /// The group's id; the client replaces the group it holds under the same id.
    /// </summary>
    [UInt(14)] public uint HexGroupId { get; set; }

    /// <summary>
    /// The group's tooltip, as a text id.
    /// </summary>
    [UInt(21)] public uint TooltipTextId { get; set; }

    /// <summary>
    /// The group's color, as an index 0 to 3; any other value gives a default color.
    /// </summary>
    [UInt(32)] public uint Color { get; set; }

    /// <summary>
    /// Whether the group shows; the client keeps the group only while it is set.
    /// </summary>
    [UInt(1)] public bool IsVisible { get; set; }
}
