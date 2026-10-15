using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.ClientCustomerSurveySubmit, Direction.ClientToServer)]
public sealed partial class ClientCustomerSurveySubmit
{
    [UInt(14)] public SurveyType CustomerSurveyId { get; private set; }
    [Union(nameof(CustomerSurveyId))] public ISurvey Survey { get; private set; }
    [WideString] public string Comment { get; private set; }
}
