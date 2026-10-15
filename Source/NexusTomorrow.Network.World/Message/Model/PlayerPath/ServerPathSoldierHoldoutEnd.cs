using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ServerPathSoldierHoldoutEnd)]
    public class ServerPathSoldierHoldoutEnd : IWritable
    {
        public ushort PathSoldierEventId { get; set; }
        public PlayerPathSoldierResult Reason { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PathSoldierEventId, 14);
            writer.Write(Reason, 32u);
        }
    }
}
