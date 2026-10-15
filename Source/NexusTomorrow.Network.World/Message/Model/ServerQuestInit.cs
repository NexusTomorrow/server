using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerQuestInit, Direction.ServerToClient)]
public sealed partial class ServerQuestInit
{
    [WireStruct]
    public sealed partial class QuestComplete
    {
        [UInt(15)] public ushort QuestId { get; set; }
        [UInt(1)] public bool CompletedToday { get; set; }
    }

    [WireStruct]
    public sealed partial class QuestInactive
    {
        [UInt(15)] public ushort QuestId { get; set; }
        [UInt(4)] public QuestState State { get; set; }
    }

    // One objective count, then every objective's progress, then the quest's elapsed time, then every objective's
    // elapsed time: element i of each objective list is one objective.
    [WireStruct]
    public sealed partial class QuestActive
    {
        [UInt(15)] public ushort QuestId { get; set; }
        [UInt(4)] public QuestState State { get; set; }
        [UInt(32)] public uint QuestObjectiveId { get; set; } // See questObjective tbl
        [UInt(32)] public QuestStateFlags Flags { get; set; }
        [UInt(32), Count] public uint ObjectiveCount { get; set; }
        [Array(CountFrom = nameof(ObjectiveCount)), UInt(32)] public List<uint> ObjectiveProgress { get; set; } = []; // Sometimes flags, sometimes numbers
        [UInt(32)] public uint QuestTimeElapsed { get; set; }
        [Array(CountFrom = nameof(ObjectiveCount)), UInt(32)] public List<uint> ObjectiveTimeElapsed { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<QuestComplete> Completed { get; set; } = [];
    [Array(CountBits = 32)] public List<QuestInactive> Inactive { get; set; } = [];
    [Array(CountBits = 32)] public List<QuestActive> Active { get; set; } = [];

    [UInt(64)] public ulong DailyRandomSeed { get; set; }
}
