using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity
{
    // Sends the direction the player must press to break out of a stun.
    [Message(GameMessageOpcode.ServerCCStateStunDirection)]
    public class ServerCCStateStunDirection : IWritable
    {
        public CCStateStunVictimGameplay Direction { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Direction, 8u);
        }
    }
}
