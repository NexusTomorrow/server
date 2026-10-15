using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ClientPathSoldierImprovementBuild)]
    public class ClientPathSoldierImprovementBuild : IReadable
    {
        public ushort PathSoldierTowerDefenseId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            PathSoldierTowerDefenseId = reader.ReadUShort(14);
        }
    }
}
