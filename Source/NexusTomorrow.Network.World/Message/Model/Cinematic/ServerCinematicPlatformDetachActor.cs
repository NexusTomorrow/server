using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematicPlatformDetachActor)]
    public class ServerCinematicPlatformDetachActor : IWritable
    {
        public uint Delay { get; set; }
        public uint UnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Delay);
            writer.Write(UnitId);
        }
    }
}
