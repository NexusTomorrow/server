using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematic022B)]
    public class ServerCinematic022B : IWritable
    {
        public uint Delay { get; set; }
        public bool Unknown1 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Delay);
            writer.Write(Unknown1);
        }
    }
}
