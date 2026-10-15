using System.Xml;

namespace NexusTomorrow.Network.Sts.Model;

public class ServerErrorMessage : IWritable
{
    private readonly int _code;

    public ServerErrorMessage(int code)
    {
        _code = code;
    }

    public void Write(XmlWriter writer)
    {
        writer.WriteStartElement("Error");

        writer.WriteStartAttribute("code");
        writer.WriteValue(_code);
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("server");
        writer.WriteValue(0); // TODO
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("module");
        writer.WriteValue(0); // TODO
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("line");
        writer.WriteValue(0); // TODO
        writer.WriteEndAttribute();

        writer.WriteStartAttribute("text");
        writer.WriteValue(0); // TODO
        writer.WriteEndAttribute();

        writer.WriteEndElement();
    }
}
