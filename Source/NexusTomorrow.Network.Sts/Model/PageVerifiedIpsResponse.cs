using System.Xml;

namespace NexusTomorrow.Network.Sts.Model;

/// <summary>
/// The answer to <c>/Auth/PageVerifiedIps</c>: the account's verified addresses, always none. The client needs only the
/// <c>Items</c> element; an empty one marks its list ready, so it stops asking.
/// </summary>
public sealed class PageVerifiedIpsResponse : IWritable
{
    /// <inheritdoc/>
    public void Write(XmlWriter writer)
    {
        writer.WriteStartElement("Reply");

        writer.WriteStartElement("Items");
        writer.WriteFullEndElement();

        writer.WriteEndElement();
    }
}
