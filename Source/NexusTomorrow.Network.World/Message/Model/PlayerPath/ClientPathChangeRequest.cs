using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ClientPathChangeRequest)]
    public class ClientPathChangeRequest : IReadable
    {
        public Path Path { get; private set; }
        public bool OnCooldown { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Path      = reader.ReadEnum<Path>(3);
            OnCooldown = reader.ReadBit();
        }
    }
}
