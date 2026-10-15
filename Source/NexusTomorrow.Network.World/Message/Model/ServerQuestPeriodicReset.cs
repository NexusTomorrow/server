using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerQuestPeriodicReset, Direction.ServerToClient)]
public sealed partial class ServerQuestPeriodicReset
{
    [UInt(64)] public ulong DailyRandomSeed { get; set; }
    [UInt(4)] public QuestRepeatPeriodFlags ResetFlags { get; set; }
}
