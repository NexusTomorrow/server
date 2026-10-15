using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info
{
    [Message(GameMessageOpcode.ClientRealmInfoRequest)]
    public class ClientRealmInfoRequest : IReadable
    {
        public uint RealmId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            RealmId = reader.ReadUInt(14u);
        }
    }
}
