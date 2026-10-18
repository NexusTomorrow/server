using System.Text;
using System.Xml;
using NexusTomorrow.Network.Sts;

namespace NexusTomorrow.Gateway.Sts;

/// <summary>
/// Writes STS responses: <c>STS/1.0 &lt;code&gt;  &lt;text&gt;</c> (two spaces), the body's UTF-8 byte length as <c>l</c>, the request's
/// sequence with <c>R</c> appended as <c>s</c>, a blank line, then the body.
/// </summary>
public static class StsResponseWriter
{
    private static readonly XmlWriterSettings s_settings = new()
    {
        OmitXmlDeclaration = true,
        Indent = true,
        IndentChars = "",
        NewLineChars = "\n",
        Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
    };

    /// <summary>
    /// The response's bytes.
    /// </summary>
    public static byte[] Write(int statusCode, string statusText, string sequence, string body)
    {
        byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
        return [.. Encoding.ASCII.GetBytes($"STS/1.0 {statusCode}  {statusText}\r\nl:{bodyBytes.Length}\r\ns:{sequence}R\r\n\r\n"), .. bodyBytes];
    }

    /// <summary>
    /// <paramref name="message"/> as the response body's XML, without the trailing newline the response adds.
    /// </summary>
    public static string Xml(IWritable message)
    {
        var text = new StringBuilder();
        using (var writer = XmlWriter.Create(text, s_settings))
        {
            writer.WriteStartDocument();
            message.Write(writer);
            writer.WriteEndDocument();
        }

        return text.ToString();
    }
}
