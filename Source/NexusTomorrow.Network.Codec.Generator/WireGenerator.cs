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
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class WireGenerator : IIncrementalGenerator
{
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

        context.RegisterSourceOutput(
            structs.Collect().Combine(messages.Collect()).Combine(assemblyName),
            static (production, input) => Emit(production, input.Left.Left, input.Left.Right, input.Right));
    }

    private static void Emit(SourceProductionContext production, ImmutableArray<TypeModel?> structs, ImmutableArray<TypeModel?> messages, string assemblyName)
    {
        var types = new Dictionary<string, TypeModel>(StringComparer.Ordinal);
        foreach (TypeModel? model in structs)
        {
            if (model is not null)
                types[model.FullName] = model;
        }

        foreach (TypeModel? model in messages)
        {
            if (model is not null)
                types[model.FullName] = model;
        }

        var layouts = new LayoutCalculator(types);
        HashSet<string> brokenHolders = CheckUnions(production, types.Values, layouts);

        var registry = new List<(TypeModel Type, MessageModel Message)>();
        var claimed = new Dictionary<(ushort Opcode, int Direction), string>();
        foreach (TypeModel type in types.Values.OrderBy(type => type.FullName, StringComparer.Ordinal))
        {
            bool failed = false;
            foreach (DiagnosticInfo diagnostic in type.Diagnostics)
            {
                production.ReportDiagnostic(diagnostic.ToDiagnostic());
                failed |= diagnostic.IsError;
            }

            failed |= brokenHolders.Contains(type.FullName);
            if (failed)
                continue;

            production.AddSource(HintName(type.FullName), SourceEmitter.EmitType(type, layouts));
            if (type.Message is not { } message)
                continue;

            bool duplicate = false;
            foreach (int direction in s_directions.Where(direction => (message.Direction & direction) != 0))
            {
                if (claimed.TryGetValue((message.Opcode, direction), out string? owner))
                {
                    string text = string.Format(CultureInfo.InvariantCulture, "'{0}' and '{1}' both declare opcode 0x{2:X4} in the same direction", owner, type.Name, message.Opcode);
                    production.ReportDiagnostic(Diagnostic.Create(Diagnostics.Duplicate, type.Location?.ToLocation(), text));
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

        production.AddSource("WireMessages.g.cs", SourceEmitter.EmitRegistry(assemblyName, [.. registry.OrderBy(entry => entry.Message.Opcode)]));
    }

    /// <summary>
    /// Report every union with no arm, with two arms for one value, or with an arm whose value its selector cannot hold, and return the full names of the types that hold one, so they emit nothing.
    /// </summary>
    private static HashSet<string> CheckUnions(SourceProductionContext production, IEnumerable<TypeModel> types, LayoutCalculator layouts)
    {
        var brokenHolders = new HashSet<string>(StringComparer.Ordinal);
        var duplicated = new Dictionary<string, bool>(StringComparer.Ordinal);
        foreach (TypeModel type in types)
        {
            foreach (MemberModel member in type.Members.Where(member => member.Shape == MemberShape.Union))
            {
                string unionType = member.UnionType ?? "";
                List<TypeModel> arms = layouts.ArmsOf(unionType);
                if (arms.Count == 0)
                {
                    production.ReportDiagnostic(Diagnostic.Create(Diagnostics.NoArms, member.Location?.ToLocation(), member.Name, type.Name, unionType.Replace("global::", "")));
                    brokenHolders.Add(type.FullName);
                    continue;
                }

                if (!duplicated.TryGetValue(unionType, out bool duplicate))
                {
                    foreach (IGrouping<ulong, TypeModel> claim in arms.GroupBy(arm => arm.UnionCaseValue).Where(group => group.Count() > 1))
                    {
                        string text = string.Format(CultureInfo.InvariantCulture, "{0} arms of {1} claim the selector value {2}: {3}", claim.Count(), unionType.Replace("global::", ""), CaseText(claim.Key), string.Join(", ", claim.Select(arm => arm.Name)));
                        production.ReportDiagnostic(Diagnostic.Create(Diagnostics.Duplicate, claim.Last().Location?.ToLocation(), text));
                        duplicate = true;
                    }

                    duplicated[unionType] = duplicate;
                }

                if (duplicate)
                    brokenHolders.Add(type.FullName);

                MemberModel? selector = type.Members.FirstOrDefault(candidate => candidate.Name == member.UnionSelector);
                if (selector?.Value?.Scalar is not { } scalar)
                    continue;

                foreach (TypeModel arm in arms.Where(arm => !ModelBuilder.CaseFits(arm.UnionCaseValue, scalar.Encoding, scalar.Bits)))
                {
                    string bits = scalar.Bits.ToString(CultureInfo.InvariantCulture);
                    production.ReportDiagnostic(Diagnostic.Create(Diagnostics.ValueTooWide, selector.Location?.ToLocation(), selector.Name, type.Name, bits, "the [UnionCase] of " + arm.Name, CaseText(arm.UnionCaseValue)));
                    brokenHolders.Add(type.FullName);
                }
            }
        }

        return brokenHolders;
    }

    /// <summary>
    /// A sign-extended [UnionCase] value as its declaration wrote it.
    /// </summary>
    private static string CaseText(ulong value)
    {
        long signed = unchecked((long)value);
        return signed < 0 ? signed.ToString(CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture);
    }

    private static string HintName(string fullName)
    {
        char[] name = [.. fullName.Replace("global::", "").Select(c => char.IsLetterOrDigit(c) || c == '.' || c == '_' ? c : '_')];
        return new string(name) + ".g.cs";
    }
}
