using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematicStart)]
    public class ServerCinematicStart : IWritable
    {
        public uint Delay { get; set; }
        public bool Show { get; set; }
        public bool Animate { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Delay);
            writer.Write(Show);
            writer.Write(Animate);
        }
    }
}
