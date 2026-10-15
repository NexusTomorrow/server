using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.ServerCustomerSurveyRequest, Direction.ServerToClient)]
public sealed partial class ServerCustomerSurveyRequest
{
    [UInt(14)] public SurveyType CustomerSurveyId { get; set; }
    [UInt(32)] public uint ObjectId { get; set; }
}
