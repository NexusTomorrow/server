using NexusTomorrow.Game.Static.Challenges;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[Message(GameMessageOpcode.ServerChallengeUpdate)]
[WireMessage(GameMessageOpcode.ServerChallengeUpdate, Direction.ServerToClient)]
public sealed partial class ServerChallengeUpdate : IWritable
{
    [WireStruct]
    public sealed partial class Challenge : IWritable
    {
        [UInt(14)] public uint ChallengeId { get; set; }
        [UInt(4)] public ChallengeType Type { get; set; }
        [UInt(32)] public uint TargetGroupId { get; set; } // See tbl targetGroup
        [UInt(32)] public uint QualifyCount { get; set; }
        [UInt(32)] public uint QualityTotal { get; set; }
        [UInt(32)] public uint CurrentCount { get; set; }
        [UInt(32)] public uint GoalCount { get; set; }
        [UInt(32)] public uint ObjectiveCompletion { get; set; } // Sometimes a number, sometimes bit flags. Depends on challege.challengeFlags in tbl
        [UInt(32)] public uint CurrentTier { get; set; }
        [UInt(32)] public uint LastRewardTier { get; set; }
        [UInt(32)] public uint CompletionCount { get; set; }
        [UInt(1)] public bool Unlocked { get; set; }
        [UInt(1)] public bool Activated { get; set; }
        [UInt(1)] public bool OnCooldown { get; set; }
        [UInt(1)] public bool LeftArea { get; set; }
        [UInt(32)] public uint TimeActivatedDt { get; set; }
        [UInt(32)] public uint TimeTotalActive { get; set; } // varies, most commonly 5 minutes
        [UInt(32)] public uint TimeCooldownDt { get; set; }
        [UInt(32)] public uint TimeTotalCooldown { get; set; } // typically 30 minutes
        [UInt(32)] public uint TimeAreaFailDt { get; set; }
        [UInt(32)] public uint TimeTotalAreaFail { get; set; } // typically 10 seconds
        [FixedArray(3), UInt(32)] public uint[] TierGoalCount { get; set; } = new uint[3];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ChallengeId, 14u);
            writer.Write(Type, 4u);
            writer.Write(TargetGroupId);
            writer.Write(QualifyCount);
            writer.Write(QualityTotal);
            writer.Write(CurrentCount);
            writer.Write(GoalCount);
            writer.Write(ObjectiveCompletion);
            writer.Write(CurrentTier);
            writer.Write(LastRewardTier);
            writer.Write(CompletionCount);
            writer.Write(Unlocked);
            writer.Write(Activated);
            writer.Write(OnCooldown);
            writer.Write(LeftArea);
            writer.Write(TimeActivatedDt);
            writer.Write(TimeTotalActive);
            writer.Write(TimeCooldownDt);
            writer.Write(TimeTotalCooldown);
            writer.Write(TimeAreaFailDt);
            writer.Write(TimeTotalAreaFail);
            foreach (uint tier in TierGoalCount)
            {
                writer.Write(tier);
            }
        }
    }

    [Array(CountBits = 32)] public List<Challenge> ActiveChallenges { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ActiveChallenges.Count);
        foreach (Challenge challenge in ActiveChallenges)
        {
            challenge.Write(writer);
        }
    }
}
