using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option
{
    [Message(GameMessageOpcode.ClientSetInputKeySet)]
    public class ClientSetInputKeySet : IReadable
    {
        public InputSets InputKeySetEnum { get; private set; }

        public void Read(GamePacketReader reader)
        {
            InputKeySetEnum = reader.ReadEnum<InputSets>(32u);
        }
    }
}
