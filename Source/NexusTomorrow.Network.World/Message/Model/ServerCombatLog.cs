using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Combat;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerCombatLog)]
    public class ServerCombatLog : IWritable
    {
        public ICombatLog CombatLog { get; set; }

        public virtual void Write(GamePacketWriter writer)
        {
            writer.Write(CombatLog.Type, 6u);
            CombatLog.Write(writer);
        }
    } 
}
