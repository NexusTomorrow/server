using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.ServerMapUpdateHexGroup, Direction.ServerToClient)]
public sealed partial class ServerMapUpdateHexGroup
{
    [UInt(14)] public uint MapZoneHexGroupId { get; set; }
    [UInt(21)] public uint TooltipLocalizedTextId { get; set; }
    [UInt(32)] public uint Color { get; set; }
    [UInt(1)] public bool IsVisible { get; set; }
}
