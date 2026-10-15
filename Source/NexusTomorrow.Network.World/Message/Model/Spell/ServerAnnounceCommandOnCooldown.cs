using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell
{
    [Message(GameMessageOpcode.ServerAnnounceCommandOnCooldown)]
    public class ServerAnnounceCommandOnCooldown : IWritable
    {
        public uint CooldownTime { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(CooldownTime);
        }
    }
}
