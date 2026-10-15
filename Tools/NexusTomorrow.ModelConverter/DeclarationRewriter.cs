using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// Writes each converted model's declaration into its source file: <c>partial</c> (and <c>sealed</c> when safe), the
/// <c>[WireMessage]</c> or <c>[WireStruct]</c> attribute after the legacy ones, the wire members gathered in wire order at the
/// first one's place with their attributes on the property's line, added counts and pads among them, <c>[NotOnWire]</c>
/// on every other instance property and field, and the regions in the type removed. The legacy methods stay.
/// </summary>
internal static class DeclarationRewriter
{
    private const string CodecNamespace = "NexusTomorrow.Network.Codec";

    public static Dictionary<SyntaxTree, string> Rewrite(Compilation compilation, List<ModelPlan> converted)
    {
        var result = new Dictionary<SyntaxTree, string>();
        foreach (IGrouping<SyntaxTree, ModelPlan> file in converted.GroupBy(plan => plan.Declaration.SyntaxTree))
        {
            SyntaxNode root = file.Key.GetRoot();
            string eol = root.ToFullString().Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
            var plans = file.ToDictionary(plan => plan.Declaration);
            HashSet<ClassDeclarationSyntax> containers = [.. plans.Keys.SelectMany(declaration => declaration.Ancestors().OfType<ClassDeclarationSyntax>()).Where(container => !plans.ContainsKey(container))];

            SyntaxNode rewritten = root.ReplaceNodes(
                plans.Keys.Concat(containers),
                (original, current) => plans.TryGetValue(original, out ModelPlan? plan)
                    ? RewriteType(current, plan, eol)
                    : AddModifiers(current, sealType: false));
            if (rewritten is CompilationUnitSyntax unit)
                rewritten = AddUsing(unit, CodecNamespace, eol);
            result[file.Key] = rewritten.ToFullString();
        }

        return result;
    }

    private static ClassDeclarationSyntax RewriteType(ClassDeclarationSyntax node, ModelPlan plan, string eol)
    {
        node = node.ReplaceTrivia(
            node.DescendantTrivia(descendIntoTrivia: true).Where(trivia => trivia.IsKind(SyntaxKind.RegionDirectiveTrivia) || trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia)),
            (_, _) => default);

        var wire = plan.Members.Where(member => !member.IsAdded).ToDictionary(member => member.Name, StringComparer.Ordinal);
        HashSet<string> offWire = [.. plan.NotOnWire];
        var gathered = new Dictionary<string, MemberDeclarationSyntax>(StringComparer.Ordinal);
        var kept = new List<MemberDeclarationSyntax>();
        int insertAt = -1;
        string indentation = "    ";
        foreach (MemberDeclarationSyntax member in node.Members)
        {
            string? name = NameOf(member);
            if (name is not null && wire.ContainsKey(name))
            {
                if (insertAt < 0)
                {
                    insertAt = kept.Count;
                    indentation = IndentationOf(member);
                }

                gathered[name] = member;
                continue;
            }

            kept.Add(name is not null && offWire.Contains(name) ? Decorate(member, "NotOnWire") : member);
        }

        List<MemberDeclarationSyntax> block = [.. plan.Members.Select(member => member.IsAdded ? Added(member, indentation, eol) : Decorate(gathered[member.Name], member.Attributes))];
        if (block.Count > 0)
        {
            if (insertAt < 0)
            {
                insertAt = kept.FindIndex(member => member is not (FieldDeclarationSyntax or ClassDeclarationSyntax or EnumDeclarationSyntax));
                if (insertAt < 0)
                    insertAt = kept.Count;
                indentation = kept.Count > 0 ? IndentationOf(kept[0]) : indentation;
                block = [.. plan.Members.Select(member => member.IsAdded ? Added(member, indentation, eol) : Decorate(gathered[member.Name], member.Attributes))];
            }

            kept.InsertRange(insertAt, block);
        }

        node = node.WithMembers(SyntaxFactory.List(kept));
        node = AddModifiers(node, plan.CanSeal);
        bool shadowed = plan.Type.GetMembers("Direction").Length > 0 || plan.Type.BaseType?.GetMembers("Direction").Length > 0;
        string direction = shadowed ? $"{CodecNamespace}.Direction." : "Direction.";
        string attribute = plan.MessageArguments is { } arguments
            ? $"WireMessage({arguments.Replace("Direction.", direction, StringComparison.Ordinal)})"
            : "WireStruct";
        return AddTypeAttribute(node, attribute, eol);
    }

    private static string? NameOf(MemberDeclarationSyntax member)
    {
        return member switch
        {
            PropertyDeclarationSyntax property when !property.Modifiers.Any(SyntaxKind.StaticKeyword) => property.Identifier.ValueText,
            FieldDeclarationSyntax { Declaration.Variables: [var variable] } field when !field.Modifiers.Any(SyntaxKind.StaticKeyword) && !field.Modifiers.Any(SyntaxKind.ConstKeyword) => variable.Identifier.ValueText,
            _ => null,
        };
    }

    /// <summary>
    /// Put an attribute list on the member's own line, after any it has.
    /// </summary>
    private static MemberDeclarationSyntax Decorate(MemberDeclarationSyntax member, string attributes)
    {
        if (attributes.Length == 0)
            return member;

        SyntaxTriviaList leading = member.GetLeadingTrivia();
        AttributeListSyntax list = ParseAttributeList(attributes).WithTrailingTrivia(SyntaxFactory.Space);
        MemberDeclarationSyntax bare = member.WithoutLeadingTrivia();
        return bare.WithAttributeLists(bare.AttributeLists.Add(list)).WithLeadingTrivia(leading);
    }

    private static MemberDeclarationSyntax Added(WireMember member, string indentation, string eol)
    {
        MemberDeclarationSyntax declaration = SyntaxFactory.ParseMemberDeclaration(member.Declaration)
            ?? throw new InvalidOperationException("An added member did not parse: " + member.Declaration);
        return declaration.WithLeadingTrivia(SyntaxFactory.Whitespace(indentation)).WithTrailingTrivia(SyntaxFactory.EndOfLine(eol));
    }

    private static AttributeListSyntax ParseAttributeList(string attributes)
    {
        var holder = (PropertyDeclarationSyntax)(SyntaxFactory.ParseMemberDeclaration($"[{attributes}] int Holder {{ get; }}")
            ?? throw new InvalidOperationException("Attributes did not parse: " + attributes));
        return holder.AttributeLists[0].WithoutTrivia();
    }

    private static string IndentationOf(SyntaxNode node)
    {
        SyntaxTriviaList leading = node.GetLeadingTrivia();
        return leading.Count > 0 && leading[^1].IsKind(SyntaxKind.WhitespaceTrivia) ? leading[^1].ToString() : "";
    }

    /// <summary>
    /// Add <c>partial</c>, and <c>sealed</c> when asked, in the order the code standard gives: access, <c>sealed</c>, <c>partial</c>.
    /// </summary>
    private static ClassDeclarationSyntax AddModifiers(ClassDeclarationSyntax node, bool sealType)
    {
        SyntaxTokenList modifiers = node.Modifiers;
        bool hadNone = modifiers.Count == 0;
        if (sealType && !modifiers.Any(SyntaxKind.SealedKeyword))
        {
            int partialAt = modifiers.IndexOf(SyntaxKind.PartialKeyword);
            SyntaxToken sealedToken = SyntaxFactory.Token(SyntaxKind.SealedKeyword).WithTrailingTrivia(SyntaxFactory.Space);
            modifiers = partialAt >= 0 ? modifiers.Insert(partialAt, sealedToken) : modifiers.Add(sealedToken);
        }

        if (!modifiers.Any(SyntaxKind.PartialKeyword))
            modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword).WithTrailingTrivia(SyntaxFactory.Space));

        if (hadNone && modifiers.Count > 0)
        {
            modifiers = modifiers.Replace(modifiers[0], modifiers[0].WithLeadingTrivia(node.Keyword.LeadingTrivia));
            node = node.WithKeyword(node.Keyword.WithLeadingTrivia());
        }

        return node.WithModifiers(modifiers);
    }

    /// <summary>
    /// Add a type attribute on its own line after the type's existing attributes.
    /// </summary>
    private static ClassDeclarationSyntax AddTypeAttribute(ClassDeclarationSyntax node, string attribute, string eol)
    {
        AttributeListSyntax list = ParseAttributeList(attribute).WithTrailingTrivia(SyntaxFactory.EndOfLine(eol));
        if (node.AttributeLists.Count > 0)
        {
            string indentation = IndentationOf(node.AttributeLists[0]);
            return node.WithAttributeLists(node.AttributeLists.Add(list.WithLeadingTrivia(SyntaxFactory.Whitespace(indentation))));
        }

        SyntaxTriviaList leading = node.GetLeadingTrivia();
        string typeIndentation = IndentationOf(node);
        ClassDeclarationSyntax bare = node.WithoutLeadingTrivia();
        bare = bare.WithAttributeLists(SyntaxFactory.SingletonList(list.WithLeadingTrivia(leading)));
        SyntaxToken first = bare.Modifiers.Count > 0 ? bare.Modifiers[0] : bare.Keyword;
        return bare.ReplaceToken(first, first.WithLeadingTrivia(SyntaxFactory.Whitespace(typeIndentation)));
    }

    /// <summary>
    /// Add a using directive in order (System first, then ordinal) when the file lacks it.
    /// </summary>
    private static CompilationUnitSyntax AddUsing(CompilationUnitSyntax unit, string name, string eol)
    {
        if (unit.Usings.Any(directive => directive.Name?.ToString() == name))
            return unit;

        // A file's first using is followed by a blank line, as the namespace below it expects.
        UsingDirectiveSyntax directive = SyntaxFactory.ParseCompilationUnit($"using {name};{eol}").Usings[0];
        if (unit.Usings.Count == 0)
            directive = directive.WithTrailingTrivia(SyntaxFactory.EndOfLine(eol), SyntaxFactory.EndOfLine(eol));
        int index = 0;
        while (index < unit.Usings.Count && Precedes(unit.Usings[index].Name?.ToString() ?? "", name))
            index++;

        if (index == 0 && unit.Usings.Count > 0)
        {
            UsingDirectiveSyntax firstUsing = unit.Usings[0];
            directive = directive.WithLeadingTrivia(firstUsing.GetLeadingTrivia());
            unit = unit.ReplaceNode(firstUsing, firstUsing.WithLeadingTrivia());
        }

        return unit.WithUsings(unit.Usings.Insert(index, directive));
    }

    private static bool Precedes(string existing, string name)
    {
        bool existingSystem = existing == "System" || existing.StartsWith("System.", StringComparison.Ordinal);
        bool nameSystem = name == "System" || name.StartsWith("System.", StringComparison.Ordinal);
        if (existingSystem != nameSystem)
            return existingSystem;
        return string.CompareOrdinal(existing, name) < 0;
    }
}
