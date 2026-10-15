using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp
{
    [Message(GameMessageOpcode.ServerDuelFailure)]
    public class ServerDuelFailure : IWritable
    {
        public DuelFailureReason Reason { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Reason, 5u);
        }
    }
}
