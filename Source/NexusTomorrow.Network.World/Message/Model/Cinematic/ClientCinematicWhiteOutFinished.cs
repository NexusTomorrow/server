using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ClientCinematicWhiteOutFinished)]
    public class ClientCinematicWhiteOutFinished : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // zero byte message
        }
    }
}
