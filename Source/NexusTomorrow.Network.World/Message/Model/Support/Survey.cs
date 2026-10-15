using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

public interface ISurvey : IWireStruct
{
};

public partial class Survey
{
    [WireStruct]
    [UnionCase(SurveyType.QuestGeneric)]
    public sealed partial class QuestDifficultySurvey : ISurvey
    {
        // Probably Quest Id
        [UInt(16)] public short ObjectId { get; set; }
        [Pad(16)] public Pad ObjectIdHigh { get; }

        // Was the quest fun? Use 1 for not fun at all, 5 for really funny.
        [UInt(8)] public short Fun { get; set; }

        // Was the quest too difficult? Use 1 for too difficult, 5 for too easy.
        [UInt(8)] public short Diffuculty { get; set; }

        // Was the quest reward worth the effort? Use 1 for it was a wast of time, 5 for totally woth it.
        [UInt(8)] public short Reward { get; set; }

        // No fourth question given
        [UInt(8)] public short Unused { get; set; }
    }

    [WireStruct]
    [UnionCase(SurveyType.TSpellQuest)]
    public sealed partial class QuestTSpellSurvey : ISurvey
    {
        // Probably Quest Id
        [UInt(16)] public short ObjectId { get; set; }
        [Pad(16)] public Pad ObjectIdHigh { get; }

        // Was the quest fun? Use 1 for not fun at all, 5 for really funny.
        [UInt(8)] public short Fun { get; set; }

        // How difficult was it to properly use the provided T-Spell for the quest? Use 1 for very difficult, 5 for easy to use.
        [UInt(8)] public short Diffuculty { get; set; }

        // Would you like to see more content of this type? Use 1 for never again, 5 for yes.
        [UInt(8)] public short WantMore { get; set; }

        // No fourth question given
        [UInt(8)] public short Unused { get; set; }
    }

    [WireStruct]
    [UnionCase(SurveyType.HoldoutQuest)]
    public sealed partial class QuestHoldoutSurvey : ISurvey
    {
        // Probably Quest Id
        [UInt(16)] public short ObjectId { get; set; }
        [Pad(16)] public Pad ObjectIdHigh { get; }

        // Was the quest fun? Use 1 for not fun at all, 5 for really funny.
        [UInt(8)] public short Fun { get; set; }

        // Was the holdout too difficult? Use 1 for too difficult, 5 for to easy.
        [UInt(8)] public short Diffuculty { get; set; }

        // Would you like to see more content of this type? Use 1 for never again, 5 for yes.
        [UInt(8)] public short WantMore { get; set; }

        // No fourth question given
        [UInt(8)] public short Unused { get; set; }
    }

    [WireStruct]
    [UnionCase(SurveyType.LevelUp)]
    public sealed partial class LevelingSurvey : ISurvey
    {
        [UInt(16)] public short Level { get; set; }
        [Pad(16)] public Pad LevelHigh { get; }

        // Was the level you just passed fun? Use 1 for not fun at all, 5 for really funny.
        [UInt(8)] public short Fun { get; set; }

        // Did you feel under- or over-powered for your level? Use 1 for under, 5 for over.
        [UInt(8)] public short Diffuculty { get; set; }

        // Do you feel your time to level was too long or too short? Use 1 for too short, 5 for too long.
        [UInt(8)] public short TimeSpend { get; set; }

        // No fourth question given
        [UInt(8)] public short Unused { get; set; }
    }

    [WireStruct]
    [UnionCase(SurveyType.GenericChallenge)]
    public sealed partial class ChallengesSurvey : ISurvey
    {
        [UInt(16)] public short ChallengeId { get; set; }
        [Pad(16)] public Pad ChallengeIdHigh { get; }

        // Was the challenge fun? Use 1 for not fun at all, 5 for really funny.
        [UInt(8)] public short Fun { get; set; }

        // Where there enough objective targets available during the challenge? Use 1 for not enough, 5 for more than enough.
        [UInt(8)] public short TargetAmount { get; set; }

        // Did you feel the rewards offered were worth the effort? Use 1 for not worth it, 5 for totally worth it.
        [UInt(8)] public short Reward { get; set; }

        // No fourth question given
        [UInt(8)] public short Unused { get; set; }
    }

}
