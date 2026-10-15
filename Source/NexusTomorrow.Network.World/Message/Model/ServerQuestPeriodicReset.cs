using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestPeriodicReset)]
[WireMessage(GameMessageOpcode.ServerQuestPeriodicReset, Direction.ServerToClient)]
public sealed partial class ServerQuestPeriodicReset : IWritable
{
    [UInt(64)] public ulong DailyRandomSeed { get; set; }
    [UInt(4)] public QuestRepeatPeriodFlags ResetFlags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DailyRandomSeed);
        writer.Write(ResetFlags, 4u);
    }
}
