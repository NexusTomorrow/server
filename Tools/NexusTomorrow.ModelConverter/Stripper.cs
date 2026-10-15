using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// Strips the legacy reader and writer code from a project's declared models. Each [WireStruct] or [WireMessage] type loses its
/// <c>Read(GamePacketReader)</c> and <c>Write(GamePacketWriter)</c>, <c>IReadable</c> and <c>IWritable</c>, and <c>[Message]</c>; an interface that
/// extends <c>IWireStruct</c> loses <c>IReadable</c> and <c>IWritable</c>. Then every member those methods used that nothing uses any more goes too, when
/// it is a [NotOnWire] member, a private field of a class or struct, or a type that is not a wire type, until nothing more falls away.
/// </summary>
internal static class Stripper
{
    private const string GamePacketReader = "NexusTomorrow.Network.GamePacketReader";
    private const string GamePacketWriter = "NexusTomorrow.Network.GamePacketWriter";
    private const string IReadable = "NexusTomorrow.Network.Message.IReadable";
    private const string IWritable = "NexusTomorrow.Network.Message.IWritable";
    private const string MessageAttribute = "NexusTomorrow.Network.Message.MessageAttribute";
    private const string NotOnWireAttribute = "NexusTomorrow.Network.Codec.NotOnWireAttribute";
    private const string WireStructAttribute = "NexusTomorrow.Network.Codec.WireStructAttribute";
    private const string WireMessageAttribute = "NexusTomorrow.Network.Message.WireMessageAttribute";
    private const string IWireStruct = "NexusTomorrow.Network.Codec.IWireStruct";

    /// <summary>
    /// The stripped text of every source file that changed, by its tree in <paramref name="compilation"/>, and what was removed.
    /// </summary>
    public static StripResult Strip(CSharpCompilation compilation)
    {
        var doomed = new HashSet<SyntaxNode>();
        var used = new HashSet<string>(StringComparer.Ordinal);
        int methods = 0;
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (TypeDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
            {
                if (model.GetDeclaredSymbol(declaration) is not INamedTypeSymbol type)
                    continue;

                if (declaration is InterfaceDeclarationSyntax)
                {
                    if (type.AllInterfaces.Any(implemented => FullName(implemented) == IWireStruct))
                        doomed.UnionWith(LegacyBases(declaration, model));
                    continue;
                }

                if (!IsWireType(type))
                    continue;

                doomed.UnionWith(LegacyBases(declaration, model));
                doomed.UnionWith(declaration.AttributeLists.SelectMany(list => list.Attributes).Where(attribute => FullName(model.GetTypeInfo(attribute).Type) == MessageAttribute));
                foreach (MethodDeclarationSyntax method in declaration.Members.OfType<MethodDeclarationSyntax>().Where(method => IsLegacyMethod(model.GetDeclaredSymbol(method))))
                {
                    doomed.Add(method);
                    methods++;
                    used.UnionWith(SymbolsUsedBy(method, model));
                }
            }
        }

        Dictionary<SyntaxTree, SyntaxNode> roots = Apply(compilation, doomed);
        var removed = new List<string>();
        while (true)
        {
            List<SyntaxTree> trees = [.. compilation.SyntaxTrees.Select(tree => roots.TryGetValue(tree, out SyntaxNode? root) ? tree.WithRootAndOptions(root, tree.Options) : tree)];
            compilation = compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(trees);
            roots = compilation.SyntaxTrees.Where(tree => roots.Keys.Any(old => old.FilePath == tree.FilePath)).ToDictionary(tree => tree, tree => tree.GetRoot());

            Dictionary<string, List<SyntaxNode>> orphans = Orphans(compilation, used);
            if (orphans.Count == 0)
                break;

            var next = new HashSet<SyntaxNode>();
            foreach ((string id, List<SyntaxNode> declarations) in orphans)
            {
                removed.Add(id);
                foreach (SyntaxNode declaration in declarations)
                {
                    next.Add(declaration);
                    used.UnionWith(SymbolsUsedBy(declaration, compilation.GetSemanticModel(declaration.SyntaxTree)));
                }
            }

            foreach ((SyntaxTree tree, SyntaxNode root) in Apply(compilation, next))
                roots[tree] = root;
        }

        var changed = roots.ToDictionary(entry => entry.Key.FilePath, entry => entry.Value.ToFullString(), StringComparer.Ordinal);
        return new StripResult(changed, methods, removed);
    }

    /// <summary>
    /// The declarations of members and nested types the removed code used that nothing uses now: [NotOnWire] members, fields, and nested types that are not
    /// wire types, each keyed by its documentation id. A virtual member and every override of it are one family: a reference to any of them keeps all of them,
    /// and they go together.
    /// </summary>
    private static Dictionary<string, List<SyntaxNode>> Orphans(CSharpCompilation compilation, HashSet<string> used)
    {
        var families = new Dictionary<string, List<SyntaxNode>>(StringComparer.Ordinal);
        foreach (string id in used)
        {
            if (DocumentationCommentId.GetFirstSymbolForDeclarationId(id, compilation) is not { } symbol || !IsRemovable(symbol))
                continue;

            SyntaxNode? declaration = symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
            if (declaration is VariableDeclaratorSyntax { Parent.Parent: FieldDeclarationSyntax field } && field.Declaration.Variables.Count == 1)
                declaration = field;
            if (declaration is MemberDeclarationSyntax)
                families[id] = [declaration];
        }

        if (families.Count == 0)
            return [];

        // Each override's id leads to the family of the member it overrides.
        var familyOf = families.Keys.ToDictionary(id => id, id => id, StringComparer.Ordinal);
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (MemberDeclarationSyntax member in tree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>().Where(member => member is PropertyDeclarationSyntax or MethodDeclarationSyntax))
            {
                if (model.GetDeclaredSymbol(member) is not { } symbol || DocumentationCommentId.CreateDeclarationId(symbol) is not { } memberId)
                    continue;

                for (ISymbol? overridden = Overridden(symbol); overridden is not null; overridden = Overridden(overridden))
                {
                    if (DocumentationCommentId.CreateDeclarationId(overridden.OriginalDefinition) is { } baseId && families.TryGetValue(baseId, out List<SyntaxNode>? family))
                    {
                        family.Add(member);
                        familyOf[memberId] = baseId;
                        break;
                    }
                }
            }
        }

        var referenced = new HashSet<string>(StringComparer.Ordinal);
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (SimpleNameSyntax name in tree.GetRoot().DescendantNodes().OfType<SimpleNameSyntax>())
            {
                if (Original(model.GetSymbolInfo(name).Symbol) is not { } symbol || DocumentationCommentId.CreateDeclarationId(symbol) is not { } id || !familyOf.TryGetValue(id, out string? familyId))
                    continue;

                // A reference from inside the family's own declarations keeps nothing alive.
                if (!families[familyId].Any(declaration => declaration.SyntaxTree == tree && declaration.FullSpan.Contains(name.Span)))
                    referenced.Add(familyId);
            }
        }

        return families.Where(entry => !referenced.Contains(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value, StringComparer.Ordinal);
    }

    private static ISymbol? Overridden(ISymbol symbol)
    {
        return symbol switch
        {
            IPropertySymbol property => property.OverriddenProperty,
            IMethodSymbol method => method.OverriddenMethod,
            _ => null,
        };
    }

    private static bool IsRemovable(ISymbol symbol)
    {
        if (symbol.Locations.All(location => !location.IsInSource) || symbol.ContainingType is null)
            return false;

        return symbol switch
        {
            IPropertySymbol or IFieldSymbol when symbol.GetAttributes().Any(attribute => FullName(attribute.AttributeClass) == NotOnWireAttribute) => true,
            IFieldSymbol { DeclaredAccessibility: Accessibility.Private, ContainingType.TypeKind: TypeKind.Class or TypeKind.Struct } => true,
            INamedTypeSymbol type => !IsWireType(type),
            _ => false,
        };
    }

    /// <summary>
    /// The documentation ids of the source members and types <paramref name="node"/> names.
    /// </summary>
    private static IEnumerable<string> SymbolsUsedBy(SyntaxNode node, SemanticModel model)
    {
        foreach (SimpleNameSyntax name in node.DescendantNodes().OfType<SimpleNameSyntax>())
        {
            if (Original(model.GetSymbolInfo(name).Symbol) is { } symbol && symbol.Locations.Any(location => location.IsInSource) && DocumentationCommentId.CreateDeclarationId(symbol) is { } id)
                yield return id;
        }
    }

    private static ISymbol? Original(ISymbol? symbol)
    {
        return symbol switch
        {
            null => null,
            IMethodSymbol { MethodKind: MethodKind.Constructor } constructor => constructor.ContainingType.OriginalDefinition,
            _ => symbol.OriginalDefinition,
        };
    }

    private static Dictionary<SyntaxTree, SyntaxNode> Apply(CSharpCompilation compilation, HashSet<SyntaxNode> doomed)
    {
        var roots = new Dictionary<SyntaxTree, SyntaxNode>();
        foreach (IGrouping<SyntaxTree, SyntaxNode> group in doomed.GroupBy(node => node.SyntaxTree))
        {
            SyntaxTree tree = compilation.SyntaxTrees.Single(candidate => candidate.FilePath == group.Key.FilePath);
            roots[tree] = new Remover([.. group]).Visit(group.Key.GetRoot()) ?? throw new InvalidOperationException($"Stripping {tree.FilePath} removed the whole file.");
        }

        return roots;
    }

    private static IEnumerable<SyntaxNode> LegacyBases(TypeDeclarationSyntax declaration, SemanticModel model)
    {
        return declaration.BaseList?.Types.Where(baseType => FullName(model.GetTypeInfo(baseType.Type).Type) is IReadable or IWritable) ?? [];
    }

    private static bool IsLegacyMethod(IMethodSymbol? method)
    {
        return method is { Parameters.Length: 1 }
            && ((method.Name == "Read" && FullName(method.Parameters[0].Type) == GamePacketReader) || (method.Name == "Write" && FullName(method.Parameters[0].Type) == GamePacketWriter));
    }

    private static bool IsWireType(INamedTypeSymbol type)
    {
        return type.GetAttributes().Any(attribute => FullName(attribute.AttributeClass) is WireStructAttribute or WireMessageAttribute);
    }

    private static string? FullName(ITypeSymbol? type)
    {
        return type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Replace("global::", "", StringComparison.Ordinal);
    }

    /// <summary>
    /// Removes the doomed nodes: a member with its trivia, a base type or an attribute from its list, and a list left empty. A declaration whose first
    /// attribute list goes keeps that list's leading trivia, which holds the comments written above the type.
    /// </summary>
    private sealed class Remover(HashSet<SyntaxNode> doomed) : CSharpSyntaxRewriter
    {
        public override SyntaxNode? Visit(SyntaxNode? node)
        {
            if (node is null || doomed.Contains(node))
                return null;

            SyntaxNode? visited = base.Visit(node);
            if (node is MemberDeclarationSyntax member && member.AttributeLists.Count > 0)
                visited = visited?.WithLeadingTrivia(member.GetLeadingTrivia());

            // A base list that goes takes the line's end with it, so the token before it gets that trivia back.
            if (node is TypeDeclarationSyntax { BaseList: { } baseList } && visited is TypeDeclarationSyntax { BaseList: null } stripped)
            {
                SyntaxToken last = stripped.ParameterList?.CloseParenToken ?? stripped.TypeParameterList?.GreaterThanToken ?? stripped.Identifier;
                visited = stripped.ReplaceToken(last, last.WithTrailingTrivia(baseList.GetTrailingTrivia()));
            }

            // When a type's first member goes, the blank line that separated the next member from it would open the body.
            if (node is TypeDeclarationSyntax { Members: [var first, ..] } && doomed.Contains(first) && visited is TypeDeclarationSyntax { Members: [var opening, ..] } result)
                visited = result.ReplaceNode(opening, opening.WithLeadingTrivia(WithoutBlankLines(opening.GetLeadingTrivia())));

            return visited;
        }

        /// <summary>
        /// <paramref name="leading"/> without the blank lines before its first comment or token, keeping that line's indentation.
        /// </summary>
        private static SyntaxTriviaList WithoutBlankLines(SyntaxTriviaList leading)
        {
            int content = 0;
            while (content < leading.Count && (leading[content].IsKind(SyntaxKind.WhitespaceTrivia) || leading[content].IsKind(SyntaxKind.EndOfLineTrivia)))
                content++;

            int line = content;
            while (line > 0 && leading[line - 1].IsKind(SyntaxKind.WhitespaceTrivia))
                line--;

            return SyntaxFactory.TriviaList(leading.Skip(line));
        }

        public override SyntaxNode? VisitBaseList(BaseListSyntax node)
        {
            List<BaseTypeSyntax> kept = [.. node.Types.Where(type => !doomed.Contains(type)).Select(type => type.WithoutTrivia())];
            if (kept.Count == node.Types.Count)
                return base.VisitBaseList(node);
            if (kept.Count == 0)
                return null;

            return node.WithTypes(SyntaxFactory.SeparatedList(kept, Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken).WithTrailingTrivia(SyntaxFactory.Space), kept.Count - 1)))
                .WithTrailingTrivia(node.GetTrailingTrivia());
        }

        public override SyntaxNode? VisitAttributeList(AttributeListSyntax node)
        {
            List<AttributeSyntax> kept = [.. node.Attributes.Where(attribute => !doomed.Contains(attribute))];
            if (kept.Count == node.Attributes.Count)
                return base.VisitAttributeList(node);
            if (kept.Count == 0)
                return null;

            return node.WithAttributes(SyntaxFactory.SeparatedList(kept));
        }
    }
}

/// <summary>
/// What a strip changed: the new text of each file, the legacy methods removed, and the documentation ids of the members and types that fell away with them.
/// </summary>
internal sealed record StripResult(IReadOnlyDictionary<string, string> Files, int Methods, IReadOnlyList<string> Removed);
