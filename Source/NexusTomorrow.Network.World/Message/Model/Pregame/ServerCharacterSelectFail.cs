using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame
{
    [Message(GameMessageOpcode.ServerCharacterSelectFail)]
    public class ServerCharacterSelectFail : IWritable
    {
        public CharacterSelectResult Result { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Result, 3u);
        }
    }
}
