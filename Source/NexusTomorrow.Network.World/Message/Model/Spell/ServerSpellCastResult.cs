using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell
{
    [Message(GameMessageOpcode.ServerSpellCastResult)]
    public class ServerSpellCastResult : IWritable
    {
        public uint ClientSpellCastUniqueId { get; set; }
        public uint Spell4Id { get; set; }
        public CastResult CastResult { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ClientSpellCastUniqueId);
            writer.Write(Spell4Id, 18u);
            writer.Write(CastResult, 9u);
        }
    }
}
