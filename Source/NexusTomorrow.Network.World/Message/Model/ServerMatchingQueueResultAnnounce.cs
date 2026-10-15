using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingQueueResultAnnounce)]
    public class ServerMatchingQueueResultAnnounce : IWritable
    {
        public MatchingQueueResult Result { get; set; }
        public MatchingQueueStatus Status { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Result, 6u);
            writer.Write(Status, 4u);
        }
    }
}
