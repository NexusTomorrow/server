using System.Xml;

namespace NexusTomorrow.Network.Sts.Model
{
    public class RequestGameTokenResponse : IWritable
    {
        public required string Token { get; set; }

        public void Write(XmlWriter writer)
        {
            writer.WriteStartElement("Reply");

            writer.WriteStartElement("Token");
            writer.WriteString(Token);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
