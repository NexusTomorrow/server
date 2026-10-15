using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestStateChange)]
public class ServerQuestStateChange : IWritable
{
    public ushort QuestId { get; set; }
    public QuestState QuestState { get; set; }
    public uint QuestObjectiveId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(QuestId, 15u);
        writer.Write(QuestState, 32u);
        writer.Write(QuestObjectiveId);
    }
}
