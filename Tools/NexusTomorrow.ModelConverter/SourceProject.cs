using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// A project's sources compiled without MSBuild: every <c>.cs</c> file outside <c>bin</c> and <c>obj</c>, the SDK's
/// implicit usings, the framework, and the assemblies in this tool's own output folder except the project's own.
/// </summary>
internal static class SourceProject
{
    /// <summary>
    /// The global usings <c>Microsoft.NET.Sdk</c> generates for <c>ImplicitUsings</c>, which live in <c>obj</c> and so are not among the sources.
    /// </summary>
    private const string ImplicitUsings = """
        global using global::System;
        global using global::System.Collections.Generic;
        global using global::System.IO;
        global using global::System.Linq;
        global using global::System.Net.Http;
        global using global::System.Threading;
        global using global::System.Threading.Tasks;
        """;

    public static CSharpCompilation Load(string projectDirectory)
    {
        string projectFile = Directory.GetFiles(projectDirectory, "*.csproj").Single();
        string assemblyName = Path.GetFileNameWithoutExtension(projectFile);
        CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);

        List<SyntaxTree> trees = [CSharpSyntaxTree.ParseText(ImplicitUsings, options, Path.Combine(projectDirectory, "obj", "ImplicitUsings.g.cs"))];
        foreach (string file in Directory.EnumerateFiles(projectDirectory, "*.cs", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
        {
            string relative = Path.GetRelativePath(projectDirectory, file);
            if (relative.StartsWith("bin" + Path.DirectorySeparatorChar, StringComparison.Ordinal) || relative.StartsWith("obj" + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                continue;

            trees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(file, Encoding.UTF8), options, file, Encoding.UTF8));
        }

        return CSharpCompilation.Create(
            assemblyName,
            trees,
            References(assemblyName),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Disable));
    }

    private static List<MetadataReference> References(string assemblyName)
    {
        string trusted = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string ?? "";
        var paths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string path in trusted.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            paths[Path.GetFileNameWithoutExtension(path)] = path;

        foreach (string path in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll"))
            paths[Path.GetFileNameWithoutExtension(path)] = path;

        // The trusted list names this tool's own dependencies too, so the project's own assembly is dropped after both are merged.
        return [.. paths
            .Where(entry => !entry.Key.StartsWith("Microsoft.CodeAnalysis", StringComparison.Ordinal)
                && entry.Key != "NexusTomorrow.ModelConverter"
                && !string.Equals(entry.Key, assemblyName, StringComparison.OrdinalIgnoreCase))
            .Select(entry => MetadataReference.CreateFromFile(entry.Value))];
    }
}
