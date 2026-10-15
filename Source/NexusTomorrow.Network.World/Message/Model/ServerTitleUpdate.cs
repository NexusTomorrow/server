using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerTitleUpdate)]
    public class ServerTitleUpdate : IWritable
    {
        public ushort TitleId { get; set; }
        public bool Alreadyowned { get; set; }
        public bool Revoked { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TitleId, 14u);
            writer.Write(Alreadyowned);
            writer.Write(Revoked);
        }
    }
}
