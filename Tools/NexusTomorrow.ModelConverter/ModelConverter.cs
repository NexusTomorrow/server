using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// What became of one legacy model.
/// </summary>
/// <param name="FullName">The type's full name.</param>
/// <param name="Converted">True when the type was declared for the codec.</param>
/// <param name="Reason">Why the type stays legacy; null when converted.</param>
internal sealed record TypeOutcome(string FullName, bool Converted, string? Reason);

/// <summary>
/// The converted source of every file that changed, and the outcome of every legacy model.
/// </summary>
internal sealed record ConversionResult(IReadOnlyDictionary<SyntaxTree, string> Rewritten, IReadOnlyList<TypeOutcome> Outcomes);

/// <summary>
/// One legacy model as the converter plans it.
/// </summary>
internal sealed class ModelPlan
{
    public required INamedTypeSymbol Type { get; init; }

    public required ClassDeclarationSyntax Declaration { get; init; }

    public List<WireMember> Members { get; set; } = [];

    public string? Failure { get; set; }

    public List<INamedTypeSymbol> Dependencies { get; } = [];

    /// <summary>
    /// The <c>[WireMessage]</c> arguments for a message, or null for a nested layout.
    /// </summary>
    public string? MessageArguments { get; set; }

    public bool CanSeal { get; set; }

    public List<string> NotOnWire { get; } = [];
}

/// <summary>
/// Converts the legacy wire models of one compilation to codec declarations. A type converts when its <c>Read</c> and
/// <c>Write</c> bodies (whichever it has) match the known patterns and agree, and every model it nests or derives from
/// converts; it keeps its legacy methods and interfaces, which the generated <c>ref BitReader</c>/<c>ref BitWriter</c>
/// methods sit beside.
/// </summary>
internal static class ModelConverter
{
    private const string ReadableName = "NexusTomorrow.Network.Message.IReadable";
    private const string WritableName = "NexusTomorrow.Network.Message.IWritable";
    private const string MessageAttributeName = "NexusTomorrow.Network.Message.MessageAttribute";
    private const string WireStructName = "NexusTomorrow.Network.Codec.WireStructAttribute";
    private const string WireMessageName = "NexusTomorrow.Network.Message.WireMessageAttribute";
    private const string ReaderName = "NexusTomorrow.Network.GamePacketReader";
    private const string WriterName = "NexusTomorrow.Network.GamePacketWriter";

    public static ConversionResult Convert(Compilation compilation)
    {
        var declarations = new List<(INamedTypeSymbol Type, ClassDeclarationSyntax Declaration, SemanticModel Model)>();
        var bases = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (ClassDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (model.GetDeclaredSymbol(declaration) is not INamedTypeSymbol type)
                    continue;

                declarations.Add((type, declaration, model));
                if (type.BaseType is { } baseType)
                    bases.Add(baseType);
            }
        }

        Dictionary<INamedTypeSymbol, ModelPlan> plans = new(SymbolEqualityComparer.Default);
        foreach ((INamedTypeSymbol type, ClassDeclarationSyntax declaration, SemanticModel model) in declarations)
        {
            if (IsLegacyModel(type))
                plans[type] = Plan(type, declaration, model, bases.Contains(type));
        }

        Close(plans);
        Dictionary<SyntaxTree, string> rewritten = DeclarationRewriter.Rewrite(compilation, [.. plans.Values.Where(plan => plan.Failure is null)]);
        List<TypeOutcome> outcomes = [.. plans.Values
            .Select(plan => new TypeOutcome(plan.Type.ToDisplayString(), plan.Failure is null, plan.Failure))
            .OrderBy(outcome => outcome.FullName, StringComparer.Ordinal)];
        return new ConversionResult(rewritten, outcomes);
    }

    /// <summary>
    /// A class that reads or writes itself through the legacy reader or writer and is not yet declared for the codec.
    /// </summary>
    private static bool IsLegacyModel(INamedTypeSymbol type)
    {
        if (type.TypeKind != TypeKind.Class || HasAttribute(type, WireStructName) || HasAttribute(type, WireMessageName))
            return false;

        return type.AllInterfaces.Any(implemented => implemented.ToDisplayString() is ReadableName or WritableName)
            || HasAttribute(type, MessageAttributeName)
            || OwnMethod(type, "Read", ReaderName) is not null
            || OwnMethod(type, "Write", WriterName) is not null;
    }

    private static ModelPlan Plan(INamedTypeSymbol type, ClassDeclarationSyntax declaration, SemanticModel model, bool isBase)
    {
        var plan = new ModelPlan { Type = type, Declaration = declaration };
        if (type.DeclaringSyntaxReferences.Length > 1)
        {
            plan.Failure = "the type is declared in more than one part";
            return plan;
        }

        if (type.IsGenericType)
        {
            plan.Failure = "the type is generic";
            return plan;
        }

        if (!type.IsAbstract && !type.InstanceConstructors.Any(constructor => constructor.Parameters.Length == 0 && constructor.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal))
        {
            plan.Failure = "the type has no constructor without parameters";
            return plan;
        }

        MethodDeclarationSyntax? write = OwnMethodSyntax(type, "Write", WriterName);
        MethodDeclarationSyntax? read = OwnMethodSyntax(type, "Read", ReaderName);
        (List<WireMember> Members, string? Failure)? written = write is null ? null : Build(WriteBodyReader.Read(model, type, write));
        (List<WireMember> Members, string? Failure)? readBack = read is null ? null : Build(ReadBodyReader.Read(model, type, read));
        plan.Failure = written?.Failure ?? readBack?.Failure;
        if (plan.Failure is not null)
            return plan;

        if (written is { } fromWrite && readBack is { } fromRead && !fromWrite.Members.SequenceEqual(fromRead.Members))
        {
            plan.Failure = $"Read and Write disagree: Write is [{Describe(fromWrite.Members)}], Read is [{Describe(fromRead.Members)}]";
            return plan;
        }

        plan.Members = written?.Members ?? readBack?.Members ?? [];
        plan.Failure = CheckMembers(plan, type);
        if (plan.Failure is not null)
            return plan;

        plan.Failure = CollectDependencies(plan, type);
        if (plan.Failure is not null)
            return plan;

        bool readable = type.AllInterfaces.Any(implemented => implemented.ToDisplayString() == ReadableName) || read is not null;
        bool writable = type.AllInterfaces.Any(implemented => implemented.ToDisplayString() == WritableName) || write is not null;
        if (MessageOpcode(type) is { } opcode)
        {
            string direction = (readable, writable) switch
            {
                (true, true) => "Both",
                (true, false) => "ClientToServer",
                _ => "ServerToClient",
            };
            plan.MessageArguments = $"{opcode}, Direction.{direction}";
        }

        // Sealing a type that declares a virtual or protected member would not compile under warnings as errors (CS0549, CS0628).
        plan.CanSeal = !type.IsAbstract && !type.IsSealed && !type.IsStatic && !isBase
            && !type.GetMembers().Any(member => member.IsVirtual || member.IsAbstract || member.DeclaredAccessibility is Accessibility.Protected or Accessibility.ProtectedOrInternal or Accessibility.ProtectedAndInternal);
        return plan;
    }

    private static (List<WireMember> Members, string? Failure) Build((List<RawOp> Ops, string? Failure) body)
    {
        return body.Failure is not null ? ([], body.Failure) : MemberBuilder.Build(body.Ops);
    }

    /// <summary>
    /// Check every wire member exists with a setter where the generated Read assigns it, and list the instance members off the wire.
    /// </summary>
    private static string? CheckMembers(ModelPlan plan, INamedTypeSymbol type)
    {
        var wire = plan.Members.Where(member => !member.IsAdded).Select(member => member.Name).ToHashSet(StringComparer.Ordinal);
        foreach (ISymbol member in type.GetMembers().Where(member => !member.IsStatic && !member.IsImplicitlyDeclared && member.Locations.Any(location => location.IsInSource)))
        {
            if (member is not (IPropertySymbol { IsIndexer: false } or IFieldSymbol { IsConst: false }))
                continue;

            if (member is IFieldSymbol { DeclaringSyntaxReferences: [{ } reference] } && reference.GetSyntax().Parent is VariableDeclarationSyntax { Variables.Count: > 1 })
                return $"the field {member.Name} shares its declaration with others";

            if (!wire.Remove(member.Name))
            {
                plan.NotOnWire.Add(member.Name);
                continue;
            }

            WireMember declared = plan.Members.First(candidate => !candidate.IsAdded && candidate.Name == member.Name);
            bool settable = member is IPropertySymbol { SetMethod: { IsInitOnly: false } } or IFieldSymbol { IsReadOnly: false };
            bool inPlace = declared.Attributes.Length == 0
                || (declared.Attributes.StartsWith("Array(", StringComparison.Ordinal) && MemberBuilder.TypeOf(member) is not IArrayTypeSymbol)
                || declared.Attributes.StartsWith("FixedArray(", StringComparison.Ordinal);
            if (!settable && !inPlace)
                return $"{member.Name} is on the wire but has no setter";
        }

        return wire.Count > 0 ? $"{string.Join(", ", wire)} is on the wire but not a member of this type" : null;
    }

    /// <summary>
    /// The models the declaration nests or derives from; a model from another assembly must already be declared.
    /// </summary>
    private static string? CollectDependencies(ModelPlan plan, INamedTypeSymbol type)
    {
        if (type.BaseType is { SpecialType: not SpecialType.System_Object } baseType && IsModelLike(baseType))
        {
            if (!baseType.Locations.Any(location => location.IsInSource) && !HasAttribute(baseType, WireStructName) && !HasAttribute(baseType, WireMessageName))
                return $"the base type {baseType.Name} is a legacy model in another assembly";
            plan.Dependencies.Add(baseType);
        }

        foreach (WireMember member in plan.Members.Where(member => !member.IsAdded))
        {
            ISymbol symbol = type.GetMembers(member.Name).FirstOrDefault() ?? FindInBases(type, member.Name)!;
            ITypeSymbol memberType = MemberBuilder.TypeOf(symbol);
            ITypeSymbol? nested = memberType switch
            {
                IArrayTypeSymbol array => array.ElementType,
                INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 } list => list.TypeArguments[0],
                _ => memberType,
            };
            if (nested is not INamedTypeSymbol { TypeKind: TypeKind.Class } named || named.SpecialType == SpecialType.System_String || !IsModelLike(named))
                continue;

            if (!named.Locations.Any(location => location.IsInSource) && !HasAttribute(named, WireStructName))
                return $"{member.Name} nests {named.Name}, a legacy model in another assembly";
            plan.Dependencies.Add(named);
        }

        return null;
    }

    /// <summary>
    /// Leave legacy every type that nests or derives from a type that stays legacy, until nothing changes.
    /// </summary>
    private static void Close(Dictionary<INamedTypeSymbol, ModelPlan> plans)
    {
        bool changed = true;
        while (changed)
        {
            changed = false;
            foreach (ModelPlan plan in plans.Values.Where(plan => plan.Failure is null))
            {
                INamedTypeSymbol? legacy = plan.Dependencies.FirstOrDefault(dependency => plans.TryGetValue(dependency, out ModelPlan? other) && other.Failure is not null);
                if (legacy is null)
                    continue;

                plan.Failure = $"it nests or derives from {legacy.Name}, which stays legacy";
                changed = true;
            }
        }
    }

    private static bool IsModelLike(INamedTypeSymbol type)
    {
        return IsLegacyModel(type) || HasAttribute(type, WireStructName) || HasAttribute(type, WireMessageName);
    }

    private static ISymbol? FindInBases(INamedTypeSymbol type, string name)
    {
        for (INamedTypeSymbol? current = type.BaseType; current is not null; current = current.BaseType)
        {
            if (current.GetMembers(name).FirstOrDefault() is { } found)
                return found;
        }

        return null;
    }

    /// <summary>
    /// The opcode expression of the type's <c>[Message]</c> attribute, as its source wrote it.
    /// </summary>
    private static string? MessageOpcode(INamedTypeSymbol type)
    {
        AttributeData? message = type.GetAttributes().FirstOrDefault(attribute => attribute.AttributeClass?.ToDisplayString() == MessageAttributeName);
        if (message?.ApplicationSyntaxReference?.GetSyntax() is AttributeSyntax { ArgumentList.Arguments: [var opcode, ..] })
            return opcode.Expression.ToString();
        return null;
    }

    private static IMethodSymbol? OwnMethod(INamedTypeSymbol type, string name, string parameterType)
    {
        return type.GetMembers(name).OfType<IMethodSymbol>().FirstOrDefault(method => method.Parameters is [var parameter] && parameter.Type.ToDisplayString() == parameterType);
    }

    private static MethodDeclarationSyntax? OwnMethodSyntax(INamedTypeSymbol type, string name, string parameterType)
    {
        return OwnMethod(type, name, parameterType)?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;
    }

    private static bool HasAttribute(ISymbol symbol, string name)
    {
        return symbol.GetAttributes().Any(attribute => attribute.AttributeClass?.ToDisplayString() == name);
    }

    private static string Describe(List<WireMember> members)
    {
        return string.Join("; ", members.Select(member => $"{member.Name} {member.Attributes}".Trim()));
    }
}
