using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueId, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueRemoveByClueId
{
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }
}
