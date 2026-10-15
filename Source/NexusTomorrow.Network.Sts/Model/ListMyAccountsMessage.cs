using System.Xml;

namespace NexusTomorrow.Network.Sts.Model
{
    [Message("/GameAccount/ListMyAccounts")]
    public class ListMyAccountsMessage : IReadable
    {
        public void Read(XmlDocument document)
        {
        }
    }
}
