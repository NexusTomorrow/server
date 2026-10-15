using NexusTomorrow.Game.Static.Challenges;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges
{
    [Message(GameMessageOpcode.ServerChallengeResult)]
    public class ServerChallengeResult : IWritable
    {
        public ushort ChallengeId { get; set; }
        public ChallengeResult Result { get; set; }
        public int Data { get; set; } // Sometimes localizedStringId, sometimes tier value

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ChallengeId, 14u);
            writer.Write(Result);
            writer.Write(Data);
        }
    }
}
