using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ServerPathSoldierSoldierBuildInvoke)]
    public class ServerPathSoldierSoldierBuildInvoke : IWritable
    {
        public uint UnitId { get; set; }
        public ushort PathSoldierEventId { get; set; }
        public List<uint> TowerDefenseIds { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(PathSoldierEventId, 14);
            writer.Write(TowerDefenseIds.Count);
            TowerDefenseIds.ForEach(id => writer.Write(id));
        }
    }
}
