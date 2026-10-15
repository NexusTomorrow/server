using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp
{
    // Starts the UI countdown. Only need to send to the participants.
    [Message(GameMessageOpcode.ServerDuelCountdown)]
    public class ServerDuelCountdown : IWritable
    {
        public uint ChallengerUnitId { get; set; }
        public uint OpponentUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ChallengerUnitId);
            writer.Write(OpponentUnitId);
        }
    }
}
