using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestPeriodicReset)]
public class ServerQuestPeriodicReset : IWritable
{
    public ulong DailyRandomSeed { get; set; }
    public QuestRepeatPeriodFlags ResetFlags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DailyRandomSeed);
        writer.Write(ResetFlags, 4u);
    }
}
