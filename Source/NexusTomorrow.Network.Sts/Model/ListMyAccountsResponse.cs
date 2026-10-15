using System.Xml;

namespace NexusTomorrow.Network.Sts.Model
{
    public class ListMyAccountsResponse : IWritable
    {
        public required string Alias { get; set; }
        public required string Created { get; set; }
        public required string GameAccountId { get; set; }

        public void Write(XmlWriter writer)
        {
            writer.WriteStartElement("Reply");
            writer.WriteAttributeString("type", "array");

            writer.WriteStartElement("GameAccount");

            writer.WriteStartElement("Alias");
            writer.WriteString("lol");
            writer.WriteEndElement();

            writer.WriteStartElement("Created");
            writer.WriteString("");
            writer.WriteEndElement();

            writer.WriteStartElement("GameAccountId");
            writer.WriteString("");
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
