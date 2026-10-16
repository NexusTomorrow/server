using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

/// <summary>
/// The summaries in an assembly's XML documentation file, by documentation id.
/// </summary>
internal static partial class XmlDocs
{
    // One lookup over every assembly given, since a message's layouts can live in another assembly; documentation ids name their
    // type in full, so they never collide. A missing file throws: a check that read no documentation must fail, not pass.
    public static Dictionary<string, string> SummariesOf(IEnumerable<Assembly> assemblies)
    {
        var summaries = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (Assembly assembly in assemblies)
        {
            string path = Path.ChangeExtension(assembly.Location, ".xml");
            foreach (XElement member in XDocument.Load(path).Descendants("member"))
            {
                string? text = member.Element("summary")?.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    summaries[member.Attribute("name")!.Value] = Whitespace().Replace(text, " ").Trim();
            }
        }

        return summaries;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
