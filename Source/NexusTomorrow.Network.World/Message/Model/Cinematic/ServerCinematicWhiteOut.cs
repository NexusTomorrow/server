using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematicWhiteOut)]
    public class ServerCinematicWhiteOut : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            // zero length message
        }
    }
}
