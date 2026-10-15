using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.ServerZoneMap, Direction.ServerToClient)]
public sealed partial class ServerZoneMap
{
    [UInt(32)] public uint ZoneMapId { get; set; }
    [Blob(32)] public byte[] ZoneMapBits { get; set; } = [];
}
