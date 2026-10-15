using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NexusTomorrow.Network;

namespace NexusTomorrow.ModelConverter.Tests;

/// <summary>
/// Compiles a legacy model against the real legacy reader and writer, converts it in memory, and reads back what came out.
/// </summary>
internal static partial class ConverterHarness
{
    private const string Usings = """
        global using global::System;
        global using global::System.Collections.Generic;
        global using global::System.Numerics;
        global using global::NexusTomorrow.Network;
        global using global::NexusTomorrow.Network.Message;
        """;

    private static readonly ImmutableArray<MetadataReference> s_references = LoadReferences();

    /// <summary>
    /// Convert <paramref name="source"/>, which must compile.
    /// </summary>
    public static ConversionResult Convert(string source)
    {
        CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);
        var compilation = CSharpCompilation.Create(
            "Models",
            [CSharpSyntaxTree.ParseText(Usings, options, "Usings.cs"), CSharpSyntaxTree.ParseText(source, options, "Model.cs")],
            s_references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Assert.Empty(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        return ModelConverter.Convert(compilation);
    }

    /// <summary>
    /// The one rewritten file with every run of whitespace collapsed to a space, so fragments match whatever the line breaks.
    /// </summary>
    public static string Converted(ConversionResult result)
    {
        return Collapse(Assert.Single(result.Rewritten).Value);
    }

    /// <summary>
    /// Assert each fragment appears, in this order, in <paramref name="text"/> once its whitespace is collapsed.
    /// </summary>
    public static void AssertInOrder(string text, params string[] fragments)
    {
        int from = 0;
        foreach (string fragment in fragments)
        {
            int at = text.IndexOf(Collapse(fragment), from, StringComparison.Ordinal);
            Assert.True(at >= 0, $"'{fragment}' is missing after position {from} in:{Environment.NewLine}{text}");
            from = at + 1;
        }
    }

    public static TypeOutcome Outcome(ConversionResult result, string name)
    {
        return Assert.Single(result.Outcomes, outcome => outcome.FullName == name);
    }

    private static string Collapse(string text)
    {
        return Whitespace().Replace(text.TrimStart('﻿'), " ").Trim();
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();

    private static ImmutableArray<MetadataReference> LoadReferences()
    {
        string trusted = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string ?? "";
        var paths = new HashSet<string>(trusted.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase)
        {
            typeof(GamePacketWriter).Assembly.Location,
        };
        return [.. paths.Select(path => MetadataReference.CreateFromFile(path))];
    }
}
