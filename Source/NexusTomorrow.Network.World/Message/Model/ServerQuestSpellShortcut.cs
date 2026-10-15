using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestSpellShortcut)]
[WireMessage(GameMessageOpcode.ServerQuestSpellShortcut, Direction.ServerToClient)]
public sealed partial class ServerQuestSpellShortcut : IWritable
{
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(3)] public uint Reason { get; set; } // Enum not determined
    [UInt(32)] public uint SourceId { get; set; } // The id of the quest, quest objective, challenge, public event,
                                                  // public event objective, or path mission that granted the spell to the player.
    [UInt(1)] public bool AddRemove { get; set; } // true = add, false = remove

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellId, 18u);
        writer.Write(Reason, 3);
        writer.Write(SourceId);
        writer.Write(AddRemove);
    }
}
