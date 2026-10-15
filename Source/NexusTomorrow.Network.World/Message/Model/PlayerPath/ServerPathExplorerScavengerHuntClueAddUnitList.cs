using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddUnitList, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueAddUnitList
{
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }
    [Array(CountBits = 32), UInt(32)] public uint[] UnitIdList { get; set; } = [];
}
