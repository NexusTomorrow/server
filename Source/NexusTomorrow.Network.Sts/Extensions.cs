using System.Xml;

namespace NexusTomorrow.Network.Sts;

public static class Extensions
{
    /// <summary>
    /// Return the child element of <paramref name="node"/> with the supplied name.
    /// </summary>
    /// <exception cref="InvalidDataException">The node has no child element with that name.</exception>
    public static XmlElement GetElement(this XmlNode node, string name)
    {
        return node[name] ?? throw new InvalidDataException($"Missing element {name}.");
    }

    /// <summary>
    /// Return the text of the child element of <paramref name="node"/> with the supplied name, converted to <typeparamref name="T"/>.
    /// </summary>
    /// <exception cref="InvalidDataException">The node has no child element with that name, or the element has no text.</exception>
    public static T GetValue<T>(this XmlNode node, string name)
    {
        string value = node.GetElement(name).FirstChild?.Value
            ?? throw new InvalidDataException($"Element {name} has no value.");

        object converted = Convert.ChangeType(value, typeof(T))
            ?? throw new InvalidOperationException($"Convert.ChangeType returned null for element {name}.");

        return (T)converted;
    }
}
