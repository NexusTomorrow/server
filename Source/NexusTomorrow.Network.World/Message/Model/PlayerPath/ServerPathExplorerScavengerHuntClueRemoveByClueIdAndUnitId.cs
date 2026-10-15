using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId
{
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
