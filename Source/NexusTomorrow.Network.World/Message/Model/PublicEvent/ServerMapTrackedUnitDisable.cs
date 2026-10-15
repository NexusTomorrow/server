using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerMapTrackedUnitDisable, Direction.ServerToClient)]
public sealed partial class ServerMapTrackedUnitDisable
{
    [UInt(32)] public uint TrackedUnitId { get; set; }
}
