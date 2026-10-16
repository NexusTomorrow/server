using System.Globalization;
using System.Reflection;
using System.Text;
using NexusTomorrow.Network.Message;
using NexusTomorrow.ProtocolReference;

// Writes Messages.md and one Messages-<area>.md per area into --out, stamped with the branch, commit and date the pages describe.
var options = new Dictionary<string, string>(StringComparer.Ordinal);
for (int i = 0; i + 1 < args.Length; i += 2)
    options[args[i]] = args[i + 1];

string[] required = ["--out", "--branch", "--commit", "--date"];
string[] missing = [.. required.Where(name => !options.ContainsKey(name))];
if (missing.Length > 0 || args.Length != required.Length * 2)
{
    Console.Error.WriteLine("Usage: NexusTomorrow.ProtocolReference --out <dir> --branch <branch> --commit <sha7> --date <yyyy-mm-dd>");
    if (missing.Length > 0)
        Console.Error.WriteLine($"Missing: {string.Join(", ", missing)}");
    return 2;
}

Assembly[] models =
[
    typeof(global::NexusTomorrow.Network.WireMessages).Assembly,
    typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly,
    typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly,
];

List<ReferenceMessage> messages = [.. models
    .SelectMany(assembly => assembly.GetTypes())
    .Where(type => type.GetCustomAttribute<WireMessageAttribute>() is not null)
    .Select(ReferenceMessage.Of)];

var walk = new LayoutWalk(models);
var docs = DocText.Load(models);
var stamp = new PageStamp(options["--branch"], options["--commit"], options["--date"]);
string output = options["--out"];
Directory.CreateDirectory(output);
var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

File.WriteAllText(Path.Combine(output, "Messages.md"), PageWriter.IndexPage(messages, stamp), encoding);
int pages = 1;
foreach (IGrouping<string, ReferenceMessage> area in messages.GroupBy(message => message.Area, StringComparer.Ordinal).OrderBy(group => group.Key, StringComparer.Ordinal))
{
    File.WriteAllText(Path.Combine(output, $"Messages-{area.Key}.md"), PageWriter.AreaPage(area.Key, [.. area], walk, docs, stamp), encoding);
    pages++;
}

Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"{messages.Count} messages; {pages} pages written to {output}"));
return 0;
