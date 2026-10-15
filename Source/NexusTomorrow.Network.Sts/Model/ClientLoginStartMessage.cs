using System.Xml;

namespace NexusTomorrow.Network.Sts.Model
{
    [Message("/Auth/LoginStart")]
    public class ClientLoginStartMessage : IReadable
    {
        public string LoginName { get; private set; } = string.Empty;
        //public string NetAddress { get; private set; }

        public void Read(XmlDocument document)
        {
            XmlElement rootNode = document.GetElement("Request");
            LoginName = rootNode.GetValue<string>("LoginName");
            //NetAddress = rootNode.GetValue<string>("NetAddress");
        }
    }
}
