using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    // If MatchQueueRsult is PersonalSurrenderCooldown or GlobalSurrenderCooldown, uses the WaitTime
    [Message(GameMessageOpcode.ServerMatchingMatchOperationResult)]
    public class ServerMatchingMatchOperationResult : IWritable
    {
        public MatchingQueueResult Result { get; set; }
        public uint WaitTimeBeforeVoteMS { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Result,6);
            writer.Write(WaitTimeBeforeVoteMS);
        }
    }
}
