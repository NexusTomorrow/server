using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support
{
    [Message(GameMessageOpcode.ServerCustomerSurveyRequest)]
    public class ServerCustomerSurveyRequest : IWritable
    {
        public SurveyType CustomerSurveyId { get; set; }
        public uint ObjectId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(CustomerSurveyId, 14);
            writer.Write(ObjectId, 32);
        }
    }
}
