using System.Globalization;
using System.Reflection;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Message;
using NexusTomorrow.ProtocolReference;

// Writes Messages.md, one Messages-<area>.md per area, Gateway-opcode-table.md and Command-layer-commands.md into --out, stamped with
// the branch, commit and date the pages describe, and removes a Messages-<area>.md no area has any more.
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

List<(string Name, string Text)> pages = [("Messages.md", PageWriter.IndexPage(messages, stamp))];
foreach (IGrouping<string, ReferenceMessage> area in messages.GroupBy(message => message.Area, StringComparer.Ordinal).OrderBy(group => group.Key, StringComparer.Ordinal))
    pages.Add(($"Messages-{area.Key}.md", PageWriter.AreaPage(area.Key, [.. area], walk, docs, stamp)));

pages.Add(("Gateway-opcode-table.md", PageWriter.OpcodeTablePage(OpcodeTable.All.Values, messages, stamp)));
pages.Add(("Command-layer-commands.md", PageWriter.CommandsPage(Registries.Commands(), stamp)));

IReadOnlyList<string> removed = ReferenceSet.Write(output, pages);
Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"{messages.Count} messages; {pages.Count} pages written to {output}"));
foreach (string name in removed)
    Console.WriteLine($"removed {name}: no area has it any more");
return 0;
