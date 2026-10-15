using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Generator.Tests;

/// <summary>
/// Compiles a source string, runs <see cref="WireGenerator"/> over it, and reports what came out.
/// </summary>
internal static class GeneratorHarness
{
    /// <summary>
    /// The name of the assembly a test source compiles into; the generated registry takes its namespace from it.
    /// </summary>
    public const string AssemblyName = "WireTests";

    private const string SourcePath = "Wire.cs";

    private static readonly ImmutableArray<MetadataReference> s_references = LoadReferences();

    /// <summary>
    /// Compile <paramref name="source"/>, run the generator over it, and return the generator's diagnostics, its sources, and the errors left after generation.
    /// </summary>
    public static GeneratorRun Run(string source)
    {
        CSharpCompilation compilation = Compile(source);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new WireGenerator())
            .RunGeneratorsAndUpdateCompilation(compilation, out Compilation output, out ImmutableArray<Diagnostic> diagnostics);
        GeneratorRunResult result = driver.GetRunResult().Results.Single();
        if (result.Exception is not null)
            throw new InvalidOperationException("The generator threw.", result.Exception);

        var sources = result.GeneratedSources.ToDictionary(generated => generated.HintName, generated => generated.SourceText.ToString(), StringComparer.Ordinal);
        ImmutableArray<Diagnostic> errors = [.. output.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)];
        return new GeneratorRun(source, diagnostics, sources, errors, output);
    }

    /// <summary>
    /// <paramref name="source"/> compiled as the test assembly, before the generator runs.
    /// </summary>
    public static CSharpCompilation Compile(string source)
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(source, CSharpParseOptions.Default, SourcePath);
        return CSharpCompilation.Create(
            AssemblyName,
            [tree],
            s_references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
    }

    /// <summary>
    /// The zero-based line of the one occurrence of <paramref name="text"/> in <paramref name="source"/>.
    /// </summary>
    public static int LineOf(string source, string text)
    {
        int index = source.IndexOf(text, StringComparison.Ordinal);
        if (index < 0 || source.IndexOf(text, index + 1, StringComparison.Ordinal) >= 0)
            throw new ArgumentException("The source must hold the text exactly once: " + text, nameof(text));

        return source.AsSpan(0, index).Count('\n');
    }

    /// <summary>
    /// The zero-based line a diagnostic points at.
    /// </summary>
    public static int LineOf(Diagnostic diagnostic)
    {
        return diagnostic.Location.GetLineSpan().StartLinePosition.Line;
    }

    /// <summary>
    /// Emit <paramref name="compilation"/>, load it into a context of its own, and call the public static method <paramref name="methodName"/> of <paramref name="typeName"/>.
    /// </summary>
    public static object? Invoke(Compilation compilation, string typeName, string methodName)
    {
        using var image = new MemoryStream();
        EmitResult emitted = compilation.Emit(image);
        if (!emitted.Success)
            throw new InvalidOperationException("The compilation did not emit: " + string.Join(Environment.NewLine, emitted.Diagnostics));

        image.Position = 0;
        var context = new AssemblyLoadContext(typeName, isCollectible: true);
        try
        {
            Assembly assembly = context.LoadFromStream(image);
            Type type = assembly.GetType(typeName) ?? throw new TypeLoadException("The compilation has no type " + typeName + ".");
            MethodInfo method = type.GetMethod(methodName) ?? throw new MissingMethodException(typeName, methodName);
            return method.Invoke(null, BindingFlags.DoNotWrapExceptions, null, null, null);
        }
        finally
        {
            context.Unload();
        }
    }

    private static ImmutableArray<MetadataReference> LoadReferences()
    {
        string trusted = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string ?? "";
        var paths = new HashSet<string>(trusted.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase)
        {
            typeof(WireStructAttribute).Assembly.Location,
            typeof(WireMessageAttribute).Assembly.Location,
        };
        return [.. paths.Select(path => MetadataReference.CreateFromFile(path))];
    }
}

/// <summary>
/// What one generator run produced.
/// </summary>
/// <param name="Source">The source the run compiled.</param>
/// <param name="Diagnostics">The diagnostics the generator reported.</param>
/// <param name="GeneratedSources">Each generated source's text, by hint name.</param>
/// <param name="CompilationErrors">The errors in the compilation after generation.</param>
/// <param name="Compilation">The compilation after generation.</param>
internal sealed record GeneratorRun(
    string Source,
    ImmutableArray<Diagnostic> Diagnostics,
    IReadOnlyDictionary<string, string> GeneratedSources,
    ImmutableArray<Diagnostic> CompilationErrors,
    Compilation Compilation);
