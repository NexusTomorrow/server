using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[Message(GameMessageOpcode.ServerCustomerSurveyRequest)]
[WireMessage(GameMessageOpcode.ServerCustomerSurveyRequest, Direction.ServerToClient)]
public sealed partial class ServerCustomerSurveyRequest : IWritable
{
    [UInt(14)] public SurveyType CustomerSurveyId { get; set; }
    [UInt(32)] public uint ObjectId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CustomerSurveyId, 14);
        writer.Write(ObjectId, 32);
    }
}
