using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    // Increments the CurrentAlly count and decrements the PendingAlly count if true
    // Increments the CurrentEnemy count and decrements the PendingEnemy count if false
    [Message(GameMessageOpcode.ServerMatchingMatchParticipantCountUpdate)]
    public class ServerMatchingMatchParticipantCountUpdate : IWritable
    {
        public bool Ally { get; set; } // true = ally, false = enemy

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Ally);
        }
    }
}
