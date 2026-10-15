using System.Xml;

namespace NexusTomorrow.Network.Sts;

public interface IReadable
{
    void Read(XmlDocument document);
}
