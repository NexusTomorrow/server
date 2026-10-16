using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

/// <summary>
/// The summaries in an assembly's XML documentation file, by documentation id.
/// </summary>
internal static partial class XmlDocs
{
    // A missing file throws: a check that read no documentation must fail, not pass.
    public static Dictionary<string, string> SummariesOf(Assembly assembly)
    {
        string path = Path.ChangeExtension(assembly.Location, ".xml");
        var summaries = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (XElement member in XDocument.Load(path).Descendants("member"))
        {
            string? text = member.Element("summary")?.Value;
            if (!string.IsNullOrWhiteSpace(text))
                summaries[member.Attribute("name")!.Value] = Whitespace().Replace(text, " ").Trim();
        }

        return summaries;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
