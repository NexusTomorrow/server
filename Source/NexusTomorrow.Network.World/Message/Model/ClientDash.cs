using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientDash)]
    public class ClientDash : IReadable
    {
        public DashDirection DashDirection { get; set; }

        public void Read(GamePacketReader reader)
        {
            DashDirection = reader.ReadEnum<DashDirection>(3);
        }
    }
}
