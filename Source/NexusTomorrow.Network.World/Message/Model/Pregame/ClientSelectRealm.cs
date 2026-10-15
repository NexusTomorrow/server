using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame
{
    [Message(GameMessageOpcode.ClientSelectRealm)]
    public class ClientSelectRealm : IReadable
    {
        public uint RealmId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            RealmId = reader.ReadUInt();
        }
    }
}
