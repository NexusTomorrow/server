using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestInit)]
[WireMessage(GameMessageOpcode.ServerQuestInit, Direction.ServerToClient)]
public sealed partial class ServerQuestInit : IWritable
{
    [WireStruct]
    public sealed partial class QuestComplete : IWritable
    {
        [UInt(15)] public ushort QuestId { get; set; }
        [UInt(1)] public bool CompletedToday { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(QuestId, 15u);
            writer.Write(CompletedToday);
        }
    }

    [WireStruct]
    public sealed partial class QuestInactive : IWritable
    {
        [UInt(15)] public ushort QuestId { get; set; }
        [UInt(4)] public QuestState State { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(QuestId, 15u);
            writer.Write(State, 4u);
        }
    }

    // One objective count, then every objective's progress, then the quest's elapsed time, then every objective's
    // elapsed time: element i of each objective list is one objective.
    [WireStruct]
    public sealed partial class QuestActive : IWritable
    {
        [UInt(15)] public ushort QuestId { get; set; }
        [UInt(4)] public QuestState State { get; set; }
        [UInt(32)] public uint QuestObjectiveId { get; set; } // See questObjective tbl
        [UInt(32)] public QuestStateFlags Flags { get; set; }
        [UInt(32), Count] public uint ObjectiveCount { get; set; }
        [Array(CountFrom = nameof(ObjectiveCount)), UInt(32)] public List<uint> ObjectiveProgress { get; set; } = []; // Sometimes flags, sometimes numbers
        [UInt(32)] public uint QuestTimeElapsed { get; set; }
        [Array(CountFrom = nameof(ObjectiveCount)), UInt(32)] public List<uint> ObjectiveTimeElapsed { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(QuestId, 15u);
            writer.Write(State, 4u);
            writer.Write(QuestObjectiveId);
            writer.Write(Flags, 32u);

            writer.Write(ObjectiveProgress.Count);

            foreach (uint progress in ObjectiveProgress)
                writer.Write(progress);

            writer.Write(QuestTimeElapsed);

            foreach (uint timeElapsed in ObjectiveTimeElapsed)
                writer.Write(timeElapsed);
        }
    }

    [Array(CountBits = 32)] public List<QuestComplete> Completed { get; set; } = [];
    [Array(CountBits = 32)] public List<QuestInactive> Inactive { get; set; } = [];
    [Array(CountBits = 32)] public List<QuestActive> Active { get; set; } = [];

    [UInt(64)] public ulong DailyRandomSeed { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Completed.Count);
        Completed.ForEach(q => q.Write(writer));

        writer.Write(Inactive.Count);
        Inactive.ForEach(q => q.Write(writer));

        writer.Write(Active.Count);
        Active.ForEach(q => q.Write(writer));

        writer.Write(DailyRandomSeed);
    }
}
