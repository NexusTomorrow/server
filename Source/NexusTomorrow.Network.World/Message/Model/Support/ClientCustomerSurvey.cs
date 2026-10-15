using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[Message(GameMessageOpcode.ClientCustomerSurveySubmit)]
[WireMessage(GameMessageOpcode.ClientCustomerSurveySubmit, Direction.ClientToServer)]
public sealed partial class ClientCustomerSurveySubmit : IReadable
{
    [UInt(14)] public SurveyType CustomerSurveyId { get; private set; }
    [Union(nameof(CustomerSurveyId))] public ISurvey Survey { get; private set; }
    [WideString] public string Comment { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CustomerSurveyId = (SurveyType)reader.ReadInt(14);

        switch (CustomerSurveyId)
        {
            case SurveyType.QuestGeneric:
                Survey = new Survey.QuestDifficultySurvey();
                break;
            case SurveyType.TSpellQuest:
                Survey = new Survey.QuestTSpellSurvey();
                break;
            case SurveyType.HoldoutQuest:
                Survey = new Survey.QuestHoldoutSurvey();
                break;
            case SurveyType.LevelUp:
                Survey = new Survey.LevelingSurvey();
                break;
            case SurveyType.GenericChallenge:
                Survey = new Survey.ChallengesSurvey();
                break;
        }

        Survey.Read(reader);

        Comment = reader.ReadWideString();
    }
}
