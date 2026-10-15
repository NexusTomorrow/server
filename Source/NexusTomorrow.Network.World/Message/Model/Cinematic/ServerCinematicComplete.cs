using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematicComplete)]
    public class ServerCinematicComplete : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            // zero byte message
        }
    }
}
