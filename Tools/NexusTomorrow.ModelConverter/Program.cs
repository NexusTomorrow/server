using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NexusTomorrow.ModelConverter;

if (args.Length is < 1 or > 5)
{
    Console.Error.WriteLine("Usage: NexusTomorrow.ModelConverter <project folder> [--report <file>] [--dry-run] [--strip]");
    return 2;
}

string project = Path.GetFullPath(args[0]);
string? reportPath = args.SkipWhile(arg => arg != "--report").Skip(1).FirstOrDefault();
bool dryRun = args.Contains("--dry-run");
bool strip = args.Contains("--strip");

CSharpCompilation compilation = SourceProject.Load(project);
List<Diagnostic> errors = [.. compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)];
if (errors.Count > 0)
{
    Console.Error.WriteLine($"{project} does not compile as loaded ({errors.Count} errors), so its symbols cannot be trusted:");
    foreach (Diagnostic error in errors.Take(20))
        Console.Error.WriteLine("  " + error);
    return 1;
}

Dictionary<string, string> files;
if (strip)
{
    StripResult stripped = Stripper.Strip(compilation);
    Console.WriteLine($"Stripped {stripped.Methods} legacy methods and {stripped.Removed.Count} members and types only they used; {stripped.Files.Count} files changed.");
    if (reportPath is not null)
        File.WriteAllLines(reportPath, stripped.Removed.Order(StringComparer.Ordinal), new UTF8Encoding(false));
    files = new Dictionary<string, string>(stripped.Files, StringComparer.Ordinal);
}
else
{
    ConversionResult result = ModelConverter.Convert(compilation);
    int converted = result.Outcomes.Count(outcome => outcome.Converted);
    Console.WriteLine($"{result.Outcomes.Count} legacy models: {converted} converted, {result.Outcomes.Count - converted} left legacy, {result.Rewritten.Count} files changed.");
    if (reportPath is not null)
        File.WriteAllText(reportPath, ConversionReport.Write(project, result), new UTF8Encoding(false));
    files = result.Rewritten.ToDictionary(entry => entry.Key.FilePath, entry => entry.Value, StringComparer.Ordinal);
}

if (dryRun || files.Count == 0)
    return 0;

foreach ((string path, string text) in files)
{
    bool bom = File.ReadAllBytes(path) is [0xEF, 0xBB, 0xBF, ..];
    File.WriteAllText(path, text, new UTF8Encoding(bom));
}

// The rewriter places attributes and members; dotnet format settles the whitespace to the house style. The rest of the
// project is formatted already, so the whole project is passed rather than a file list too long for one command line.
string projectFile = Directory.GetFiles(project, "*.csproj").Single();
var format = new ProcessStartInfo("dotnet", ["format", "whitespace", projectFile]);
using var formatting = Process.Start(format);
formatting?.WaitForExit();
return formatting?.ExitCode ?? 1;
