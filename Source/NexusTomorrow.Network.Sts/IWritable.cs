using System.Xml;

namespace NexusTomorrow.Network.Sts
{
    public interface IWritable
    {
        void Write(XmlWriter writer);
    }
}
