using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame
{
    [Message(GameMessageOpcode.ServerMaxCharacterLevelAchieved)]
    public class ServerMaxCharacterLevelAchieved : IWritable
    {
        public uint Level { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Level);
        }
    }
}
