using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathSoldierImprovementBuild, Direction.ClientToServer)]
public sealed partial class ClientPathSoldierImprovementBuild
{
    [UInt(14)] public ushort PathSoldierTowerDefenseId { get; private set; }
}
