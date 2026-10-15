using System.Xml;

namespace NexusTomorrow.Network.Sts.Model;

[Message("/Auth/LoginFinish")]
public class ClientLoginFinishMessage : IReadable
{
    public void Read(XmlDocument document)
    {
    }
}
