using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame
{
    [Message(GameMessageOpcode.ServerForceKick)]
    public class ServerForceKick : IWritable
    {
        public ForceKickReason Reason { get; set; } = ForceKickReason.WorldDisconnect;

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Reason, 5u);
        }
    }
}
