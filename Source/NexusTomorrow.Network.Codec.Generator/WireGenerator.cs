using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// Implements <c>IWireStruct</c> for every [WireStruct] class and <c>IWireMessage</c> for every [WireMessage] class, and writes the assembly's message registry.
/// The sources and diagnostics come from the models alone, so they cache between runs; only placing each diagnostic on its syntax tree, which lets
/// <c>.editorconfig</c> severities and <c>#pragma</c> reach it, reads the compilation.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class WireGenerator : IIncrementalGenerator
{
    /// <summary>
    /// The tracking name of the step that analyzes every wire type together.
    /// </summary>
    public const string ProductionStep = "Production";

    /// <summary>
    /// The tracking name of the step that takes the generated sources from the analysis.
    /// </summary>
    public const string SourcesStep = "Sources";

    private static readonly int[] s_directions = [1, 2];

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Both attributes target classes, which admits a record class; ModelBuilder rejects it with NTC012.
        IncrementalValuesProvider<TypeModel?> structs = context.SyntaxProvider.ForAttributeWithMetadataName(
            WireNames.WireStruct,
            static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
            static (syntax, cancellationToken) => ModelBuilder.Build(syntax, isMessage: false, cancellationToken));
        IncrementalValuesProvider<TypeModel?> messages = context.SyntaxProvider.ForAttributeWithMetadataName(
            WireNames.WireMessage,
            static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
            static (syntax, cancellationToken) => ModelBuilder.Build(syntax, isMessage: true, cancellationToken));
        IncrementalValueProvider<string> assemblyName = context.CompilationProvider.Select(static (compilation, _) => compilation.AssemblyName ?? "Wire");

        // The tracking names let a test see a run reuse the analysis and the sources.
        IncrementalValueProvider<Production> production = structs.Collect().Combine(messages.Collect()).Combine(assemblyName)
            .Select(static (input, _) => Analyze(input.Left.Left, input.Left.Right, input.Right))
            .WithTrackingName(ProductionStep);

        context.RegisterSourceOutput(
            production.Select(static (result, _) => result.Sources).WithTrackingName(SourcesStep),
            static (output, sources) =>
            {
                foreach (GeneratedSource source in sources)
                    output.AddSource(source.HintName, source.Text);
            });
        context.RegisterSourceOutput(
            production.Select(static (result, _) => result.Diagnostics).Combine(context.CompilationProvider),
            static (output, input) => Report(output, input.Left, input.Right));
    }

    /// <summary>
    /// Report each diagnostic on the syntax tree its file path names, or at the file path when no tree has it.
    /// </summary>
    private static void Report(SourceProductionContext output, EquatableArray<DiagnosticInfo> diagnostics, Compilation compilation)
    {
        if (diagnostics.Count == 0)
            return;

        var trees = new Dictionary<string, SyntaxTree>(StringComparer.Ordinal);
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
            trees[tree.FilePath] = tree;
        foreach (DiagnosticInfo diagnostic in diagnostics)
            output.ReportDiagnostic(diagnostic.ToDiagnostic(trees));
    }

    private static Production Analyze(ImmutableArray<TypeModel?> structs, ImmutableArray<TypeModel?> messages, string assemblyName)
    {
        var diagnostics = new List<DiagnosticInfo>();
        var types = new Dictionary<string, TypeModel>(StringComparer.Ordinal);
        foreach (TypeModel? model in structs)
        {
            if (model is not null)
                types[model.FullName] = model;
        }

        var failed = new HashSet<string>(StringComparer.Ordinal);
        foreach (TypeModel? model in messages)
        {
            if (model is null)
                continue;

            if (types.ContainsKey(model.FullName))
            {
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, model.Location, model.Name, "a type is a [WireStruct] or a [WireMessage], not both; a message nests as it is"));
                failed.Add(model.FullName);
            }

            types[model.FullName] = model;
        }

        List<TypeModel> ordered = [.. types.Values.OrderBy(type => type.FullName, StringComparer.Ordinal)];
        foreach (TypeModel type in ordered)
        {
            foreach (DiagnosticInfo diagnostic in type.Diagnostics)
            {
                diagnostics.Add(diagnostic);
                if (diagnostic.IsError)
                    failed.Add(type.FullName);
            }
        }

        var layouts = new LayoutCalculator(types);
        failed.UnionWith(CheckUnions(diagnostics, ordered, layouts));
        FailDependents(ordered, failed);

        var sources = new List<GeneratedSource>();
        var registry = new List<(TypeModel Type, MessageModel Message)>();
        var claimed = new Dictionary<(ushort Opcode, int Direction), string>();
        foreach (TypeModel type in ordered)
        {
            if (failed.Contains(type.FullName))
                continue;

            sources.Add(new GeneratedSource(HintName(type.FullName), SourceEmitter.EmitType(type, layouts)));
            if (type.Message is not { } message)
                continue;

            bool duplicate = false;
            foreach (int direction in s_directions.Where(direction => (message.Direction & direction) != 0))
            {
                if (claimed.TryGetValue((message.Opcode, direction), out string? owner))
                {
                    string text = string.Format(CultureInfo.InvariantCulture, "'{0}' and '{1}' both declare opcode 0x{2:X4} in the same direction", owner, type.Name, message.Opcode);
                    diagnostics.Add(DiagnosticInfo.Create(Diagnostics.Duplicate, type.Location, text));
                    duplicate = true;
                }
                else
                {
                    claimed[(message.Opcode, direction)] = type.Name;
                }
            }

            if (!duplicate)
                registry.Add((type, message));
        }

        sources.Add(new GeneratedSource("WireMessages.g.cs", SourceEmitter.EmitRegistry(assemblyName, [.. registry.OrderBy(entry => entry.Message.Opcode)])));
        return new Production(new EquatableArray<GeneratedSource>([.. sources]), new EquatableArray<DiagnosticInfo>([.. diagnostics]));
    }

    /// <summary>
    /// Report every union with no arm, with an arm its selector cannot carry, or with two arms its selector cannot tell apart, and return the full names of the types that hold one, so they emit nothing.
    /// </summary>
    private static HashSet<string> CheckUnions(List<DiagnosticInfo> diagnostics, IEnumerable<TypeModel> types, LayoutCalculator layouts)
    {
        var brokenHolders = new HashSet<string>(StringComparer.Ordinal);
        var reported = new HashSet<(string UnionType, ulong Label)>();
        foreach (TypeModel type in types)
        {
            foreach (MemberModel member in type.Members.Where(member => member.Shape == MemberShape.Union))
            {
                string unionType = member.UnionType ?? "";
                List<TypeModel> arms = layouts.ArmsOf(unionType);
                if (arms.Count == 0)
                {
                    diagnostics.Add(DiagnosticInfo.Create(Diagnostics.NoArms, member.Location, member.Name, type.Name, unionType.Replace("global::", "")));
                    brokenHolders.Add(type.FullName);
                    continue;
                }

                MemberModel? selector = type.Members.FirstOrDefault(candidate => candidate.Name == member.UnionSelector);
                if (selector?.Value?.Scalar is not { } scalar)
                    continue;

                foreach (TypeModel arm in arms.Where(arm => !UnionCases.Fits(arm.UnionCaseValue, scalar)))
                {
                    string bits = scalar.Bits.ToString(CultureInfo.InvariantCulture);
                    diagnostics.Add(DiagnosticInfo.Create(Diagnostics.CaseTooWide, selector.Location, selector.Name, type.Name, bits, arm.Name, UnionCases.ValueText(arm.UnionCaseValue)));
                    brokenHolders.Add(type.FullName);
                }

                IEnumerable<IGrouping<ulong, TypeModel>> claims = arms
                    .Where(arm => UnionCases.Fits(arm.UnionCaseValue, scalar))
                    .GroupBy(arm => UnionCases.Label(arm.UnionCaseValue, scalar))
                    .Where(group => group.Count() > 1);
                foreach (IGrouping<ulong, TypeModel> claim in claims)
                {
                    brokenHolders.Add(type.FullName);
                    if (!reported.Add((unionType, claim.Key)))
                        continue;

                    string text = string.Format(CultureInfo.InvariantCulture, "{0} arms of {1} claim the selector value {2}: {3}", claim.Count(), unionType.Replace("global::", ""), UnionCases.LabelText(claim.First().UnionCaseValue, scalar), string.Join(", ", claim.Select(arm => arm.Name)));
                    diagnostics.Add(DiagnosticInfo.Create(Diagnostics.Duplicate, claim.Last().Location, text));
                }
            }
        }

        return brokenHolders;
    }

    /// <summary>
    /// Add to <paramref name="failed"/> every type whose generated code would call into a type that emits nothing: through its base, a member, an element, or an abstract class it holds as a union.
    /// </summary>
    private static void FailDependents(List<TypeModel> types, HashSet<string> failed)
    {
        bool changed = true;
        while (changed)
        {
            changed = false;
            foreach (TypeModel type in types)
            {
                if (!failed.Contains(type.FullName) && DependenciesOf(type).Any(failed.Contains))
                    changed |= failed.Add(type.FullName);
            }
        }
    }

    private static IEnumerable<string> DependenciesOf(TypeModel type)
    {
        if (type.WireBase is not null)
            yield return type.WireBase;

        foreach (MemberModel member in type.Members)
        {
            if (member.Value?.StructType is { } structType)
                yield return structType;
            if (member.Shape == MemberShape.Union && member.UnionType is not null)
                yield return member.UnionType;
        }
    }

    private static string HintName(string fullName)
    {
        char[] name = [.. fullName.Replace("global::", "").Select(c => char.IsLetterOrDigit(c) || c == '.' || c == '_' ? c : '_')];
        return new string(name) + ".g.cs";
    }
}
