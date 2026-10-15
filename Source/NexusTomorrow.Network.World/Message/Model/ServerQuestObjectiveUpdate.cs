using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestObjectiveUpdate)]
[WireMessage(GameMessageOpcode.ServerQuestObjectiveUpdate, Direction.ServerToClient)]
public sealed partial class ServerQuestObjectiveUpdate : IWritable
{
    [UInt(15)] public ushort QuestId { get; set; }
    [UInt(32)] public uint QuestObjectiveIndex { get; set; } // Not to be confused with QuestObjectiveId for tbl records
    [UInt(32)] public uint Completed { get; set; } // Objective completion flags, can represent different types of data i.e. not always flags, sometimes a number

    public void Write(GamePacketWriter writer)
    {
        writer.Write(QuestId, 15u);
        writer.Write(QuestObjectiveIndex);
        writer.Write(Completed);
    }
}
