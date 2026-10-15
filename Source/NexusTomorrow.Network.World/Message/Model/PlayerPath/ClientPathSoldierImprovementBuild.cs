using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathSoldierImprovementBuild)]
[WireMessage(GameMessageOpcode.ClientPathSoldierImprovementBuild, Direction.ClientToServer)]
public sealed partial class ClientPathSoldierImprovementBuild : IReadable
{
    [UInt(14)] public ushort PathSoldierTowerDefenseId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PathSoldierTowerDefenseId = reader.ReadUShort(14);
    }
}
