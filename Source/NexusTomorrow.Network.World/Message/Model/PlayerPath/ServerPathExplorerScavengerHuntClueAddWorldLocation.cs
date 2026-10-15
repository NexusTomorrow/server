using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddWorldLocation, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueAddWorldLocation
{
    [UInt(17)] public uint WorldLocation2Id { get; set; }
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }
}
