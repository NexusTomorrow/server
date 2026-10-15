using System.Xml;

namespace NexusTomorrow.Network.Sts.Model;

[Message("/Auth/RequestGameToken")]
public class RequestGameTokenMessage : IReadable
{
    public void Read(XmlDocument document)
    {
    }
}
