using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerEntityFaction)]
    public class ServerEntityFaction : IWritable
    {
        public uint UnitId { get; set; }
        public Faction Faction { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(Faction, 14u);
        }
    }
}
