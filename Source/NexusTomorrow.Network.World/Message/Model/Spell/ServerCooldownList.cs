using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell
{
    [Message(GameMessageOpcode.ServerCooldownList)]
    public class ServerCooldownList : IWritable
    {
        public List<Cooldown> Cooldowns { get; set; } = new();

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Cooldowns.Count);
            Cooldowns.ForEach(c => c.Write(writer));
        }
    }
}
