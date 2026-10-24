using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0848, Direction.ServerToClient)]
public sealed partial class MapTrackedUnitRemoved
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
