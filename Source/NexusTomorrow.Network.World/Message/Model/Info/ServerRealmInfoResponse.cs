using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info
{
    [Message(GameMessageOpcode.ServerRealmInfoResponse)]
    public class ServerRealmInfoResponse : IWritable
    {
        public uint RealmId { get; set; }
        public string RealmName { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(RealmId);
            writer.WriteStringWide(RealmName);
        }
    }
}
