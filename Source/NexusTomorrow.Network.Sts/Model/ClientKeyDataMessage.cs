using System.Xml;

namespace NexusTomorrow.Network.Sts.Model;

[Message("/Auth/KeyData")]
public class ClientKeyDataMessage : IReadable
{
    public byte[] A { get; private set; } = [];
    public byte[] M1 { get; private set; } = [];

    public void Read(XmlDocument document)
    {
        XmlElement rootNode = document.GetElement("Request");
        string keyData = rootNode.GetValue<string>("KeyData");

        using var stream = new MemoryStream(Convert.FromBase64String(keyData));
        using var reader = new BinaryReader(stream);
        A = ReadPart(stream, reader);
        M1 = ReadPart(stream, reader);
    }

    // A length that claims more bytes than remain is refused before anything is allocated for it.
    private static byte[] ReadPart(MemoryStream stream, BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length < 0 || length > stream.Length - stream.Position)
            throw new InvalidDataException("KeyData holds a part longer than the data.");

        return reader.ReadBytes(length);
    }
}
