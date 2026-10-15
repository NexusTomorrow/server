using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ServerPathUpdateXP)]
    public class ServerPathUpdateXP : IWritable
    {
        public uint TotalXP { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TotalXP);
        }
    }
}
