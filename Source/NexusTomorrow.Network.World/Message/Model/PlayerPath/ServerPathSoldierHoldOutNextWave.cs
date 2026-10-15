using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ServerPathSoldierHoldOutNextWave)]
    public class ServerPathSoldierHoldOutNextWave : IWritable
    {
        public ushort PathSoldierEventId { get; set; }
        public uint WaveIndex { get; set; }
        public bool IsBoss { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PathSoldierEventId, 14);
            writer.Write(WaveIndex);
            writer.Write(IsBoss);
;        }
    }
}
