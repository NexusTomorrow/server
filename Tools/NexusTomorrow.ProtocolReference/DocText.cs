using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// The summaries and remarks in the model assemblies' documentation files, by documentation id, each as one line of Markdown:
/// whitespace collapsed, <c>&lt;c&gt;x&lt;/c&gt;</c> as <c>`x`</c>, and a <c>&lt;see cref&gt;</c> as the short name in backticks.
/// </summary>
public sealed partial class DocText
{
    private readonly Dictionary<string, string> _summaries;
    private readonly Dictionary<string, string> _remarks;

    private DocText(Dictionary<string, string> summaries, Dictionary<string, string> remarks)
    {
        _summaries = summaries;
        _remarks = remarks;
    }

    /// <summary>
    /// Read the documentation files beside <paramref name="assemblies"/>. Ids name their type in full, so one lookup serves every assembly.
    /// </summary>
    /// <exception cref="FileNotFoundException">An assembly has no documentation file beside it: pages without meanings must not pass for complete.</exception>
    public static DocText Load(IEnumerable<Assembly> assemblies)
    {
        var summaries = new Dictionary<string, string>(StringComparer.Ordinal);
        var remarks = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (Assembly assembly in assemblies)
        {
            string path = Path.ChangeExtension(assembly.Location, ".xml");
            foreach (XElement member in XDocument.Load(path).Descendants("member"))
            {
                string? id = member.Attribute("name")?.Value;
                if (id is null)
                    continue;
                if (member.Element("summary") is { } summary && Markdown(summary) is { Length: > 0 } summaryText)
                    summaries[id] = summaryText;
                if (member.Element("remarks") is { } remark && Markdown(remark) is { Length: > 0 } remarkText)
                    remarks[id] = remarkText;
            }
        }

        return new DocText(summaries, remarks);
    }

    /// <summary>
    /// The summary for <paramref name="id"/>, or null.
    /// </summary>
    public string? Summary(string? id) => id is not null && _summaries.TryGetValue(id, out string? text) ? text : null;

    /// <summary>
    /// The remarks for <paramref name="id"/>, or null.
    /// </summary>
    public string? Remarks(string? id) => id is not null && _remarks.TryGetValue(id, out string? text) ? text : null;

    private static string Markdown(XElement element)
    {
        var text = new StringBuilder();
        foreach (XNode node in element.Nodes())
            Append(text, node);
        return Whitespace().Replace(text.ToString(), " ").Trim();
    }

    private static void Append(StringBuilder text, XNode node)
    {
        switch (node)
        {
            case XText plain:
                text.Append(plain.Value);
                break;
            case XElement { Name.LocalName: "c" } code:
                text.Append('`').Append(code.Value).Append('`');
                break;
            case XElement { Name.LocalName: "see" or "seealso" } reference:
                string target = reference.Attribute("cref")?.Value ?? reference.Attribute("langword")?.Value ?? reference.Value;
                text.Append('`').Append(ShortName(target)).Append('`');
                break;
            case XElement { Name.LocalName: "paramref" or "typeparamref" } parameter:
                text.Append('`').Append(parameter.Attribute("name")?.Value ?? parameter.Value).Append('`');
                break;
            case XElement other:
                foreach (XNode inner in other.Nodes())
                    Append(text, inner);
                break;
        }
    }

    // T:A.B.C gives C, P:A.B.C.Name gives Name; a method's parameter list is dropped first.
    private static string ShortName(string target)
    {
        string name = target.Length > 2 && target[1] == ':' ? target[2..] : target;
        int parameters = name.IndexOf('(', StringComparison.Ordinal);
        if (parameters >= 0)
            name = name[..parameters];
        int dot = name.LastIndexOf('.');
        return dot >= 0 ? name[(dot + 1)..] : name;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}

/// <summary>
/// Documentation ids, as the compiler writes them in a documentation file.
/// </summary>
public static class DocumentationIds
{
    /// <summary>
    /// The id of <paramref name="type"/>: <c>T:</c> and its full name, nested types joined by dots.
    /// </summary>
    public static string Of(Type type) => "T:" + FullName(type);

    /// <summary>
    /// The id of a property (<c>P:</c>) or field (<c>F:</c>).
    /// </summary>
    public static string Of(MemberInfo member) =>
        (member is FieldInfo ? "F:" : "P:") + FullName(member.DeclaringType!) + "." + member.Name;

    private static string FullName(Type type) => (type.FullName ?? type.Name).Replace('+', '.');
}
