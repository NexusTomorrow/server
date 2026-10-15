using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility
{
    [Message(GameMessageOpcode.ServerRandomRollResponse)]
    public class ServerRandomRollResponse : IWritable
    {
        public Identity RollerIdentity { get; set; }
        public uint MinRandom { get; set; }
        public uint MaxRandom { get; set; }
        public int RandomRollResult { get; set; }

        public void Write(GamePacketWriter writer)
        {
            RollerIdentity.Write(writer);
            writer.Write(MinRandom);
            writer.Write(MaxRandom);
            writer.Write(RandomRollResult);
        }
    }
}
