using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematicMissileActorRemove)]
    public class ServerCinematicMissileActorRemove : IWritable
    {
        public uint Delay { get; set; }
        public uint MissileActorUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Delay);
            writer.Write(MissileActorUnitId);
        }
    }
}
