using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerPlayerChanged)]
    public class ServerPlayerChanged : IWritable
    {
        public uint Guid { get; set; }
        public uint Unknown1 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Guid);
            writer.Write(Unknown1);
        }
    }
}
