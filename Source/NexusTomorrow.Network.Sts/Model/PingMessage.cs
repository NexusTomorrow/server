using System.Xml;

namespace NexusTomorrow.Network.Sts.Model;

[Message("/Sts/Ping")]
public class PingMessage : IReadable
{
    public void Read(XmlDocument document)
    {
    }
}
