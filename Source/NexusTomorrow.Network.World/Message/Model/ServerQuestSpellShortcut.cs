using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerQuestSpellShortcut)]
    public class ServerQuestSpellShortcut : IWritable
    {
        public uint SpellId { get; set; }
        public uint Reason { get; set; } // Enum not determined
        public uint SourceId { get; set; } // The id of the quest, quest objective, challenge, public event,
                                           // public event objective, or path mission that granted the spell to the player.
        public bool AddRemove { get; set; } // true = add, false = remove

        public void Write(GamePacketWriter writer)
        {
            writer.Write(SpellId, 18u);
            writer.Write(Reason, 3);
            writer.Write(SourceId);
            writer.Write(AddRemove);
        }
    }
}
