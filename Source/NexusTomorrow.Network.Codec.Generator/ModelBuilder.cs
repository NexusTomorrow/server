using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// Turns one [WireStruct] or [WireMessage] class into a <see cref="TypeModel"/>, with the diagnostics its declaration earns.
/// </summary>
internal static class ModelBuilder
{
    private static readonly SymbolDisplayFormat s_fullName = SymbolDisplayFormat.FullyQualifiedFormat;

    private static readonly string[] s_scalarAttributes = [WireNames.UInt, WireNames.SInt, WireNames.Float32, WireNames.Float64, WireNames.PackedFloat];
    private static readonly string[] s_stringAttributes = [WireNames.WideString, WireNames.NarrowString, WireNames.TerminatedWideString];

    public static TypeModel? Build(GeneratorAttributeSyntaxContext context, bool isMessage, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol type || context.TargetNode is not TypeDeclarationSyntax declaration)
            return null;

        var diagnostics = new List<DiagnosticInfo>();
        Location typeLocation = declaration.Identifier.GetLocation();
        string typeName = type.Name;

        bool allPartial = IsPartial(type, cancellationToken);
        bool generic = type.IsGenericType;
        for (INamedTypeSymbol? containing = type.ContainingType; containing is not null; containing = containing.ContainingType)
        {
            allPartial &= IsPartial(containing, cancellationToken);
            generic |= containing.IsGenericType;
        }

        if (!allPartial)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.NotPartial, typeLocation, typeName));
        if (type.IsRecord)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "a wire type must be a class, not a record"));
        if (generic)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "a wire type cannot be generic or nested in a generic type"));
        if (isMessage && type.IsAbstract)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "a [WireMessage] cannot be abstract, because the registry creates it by opcode"));

        // Generated code creates a nested type, a union arm or a message with new(); an abstract type is only ever a base or a union.
        bool creatable = type.InstanceConstructors.Any(constructor => constructor.Parameters.Length == 0
            && constructor.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal or Accessibility.ProtectedOrInternal);
        if (!type.IsAbstract && !creatable)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "a wire type needs a public or internal constructor without parameters"));

        var containingTypes = new List<ContainingTypeModel>();
        for (INamedTypeSymbol? containing = type.ContainingType; containing is not null; containing = containing.ContainingType)
            containingTypes.Insert(0, new ContainingTypeModel(KeywordOf(containing), containing.Name));

        string? wireBase = null;
        LayoutModel? wireBaseLayout = null;
        if (type.BaseType is { } baseType && (HasAttribute(baseType, WireNames.WireStruct) || HasAttribute(baseType, WireNames.WireMessage)))
        {
            wireBase = baseType.ToDisplayString(s_fullName);
            if (!baseType.Locations.Any(location => location.IsInSource))
                wireBaseLayout = ReadLayoutData(baseType);
        }

        MessageModel? message = null;
        if (isMessage)
            message = BuildMessage(context.Attributes[0], typeName, typeLocation, diagnostics);

        AttributeData? unionCase = FindAttribute(type.GetAttributes(), WireNames.UnionCase);
        ulong unionCaseValue = 0;
        if (unionCase is not null && !TryGetCaseValue(ArgumentOf(unionCase, 0), out unionCaseValue))
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "[UnionCase] needs an enum value or an integer"));

        // An arm starts wherever its selector ends, so an aligned blob in it would align to the body's bytes while its measure aligns to the arm's.
        if (unionCase is not null && HoldsAlignedBlob(type))
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "a [UnionCase] arm cannot hold an aligned blob, since an arm starts wherever its selector ends"));

        var supertypes = new List<string>();
        for (INamedTypeSymbol? ancestor = type.BaseType; ancestor is not null && ancestor.SpecialType != SpecialType.System_Object; ancestor = ancestor.BaseType)
            supertypes.Add(ancestor.ToDisplayString(s_fullName));
        foreach (INamedTypeSymbol implemented in type.AllInterfaces)
            supertypes.Add(implemented.ToDisplayString(s_fullName));

        List<ISymbol> wireSymbols = WireMembersOf(type);
        if (DeclarationsHolding(wireSymbols, cancellationToken) > 1)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.SplitAcrossParts, typeLocation, typeName));

        var members = new List<MemberModel>();
        foreach (ISymbol symbol in wireSymbols)
        {
            MemberModel? member = BuildMember(symbol, typeName, isMessage, diagnostics);
            if (member is not null)
                members.Add(member);
        }

        CheckReferences(members, typeName, diagnostics);

        return new TypeModel(
            type.ToDisplayString(s_fullName),
            type.ContainingNamespace.IsGlobalNamespace ? "" : type.ContainingNamespace.ToDisplayString(),
            type.Name,
            KeywordOf(type),
            new EquatableArray<ContainingTypeModel>([.. containingTypes]),
            type.IsSealed,
            wireBase,
            wireBaseLayout,
            message,
            unionCase is not null,
            unionCaseValue,
            new EquatableArray<string>([.. supertypes]),
            new EquatableArray<MemberModel>([.. members]),
            new EquatableArray<DiagnosticInfo>([.. diagnostics]),
            LocationInfo.From(typeLocation));
    }

    /// <summary>
    /// Read the <c>[WireLayoutData]</c> the generator put on a type compiled into another assembly.
    /// </summary>
    public static LayoutModel? ReadLayoutData(INamedTypeSymbol type)
    {
        AttributeData? data = FindAttribute(type.GetAttributes(), WireNames.WireLayoutData);
        if (data is null || data.ConstructorArguments.Length != 4)
            return null;

        bool isFixed = data.ConstructorArguments[0].Value is true;
        bool isComplete = data.ConstructorArguments[1].Value is true;
        byte[] widths = [.. data.ConstructorArguments[2].Values.Select(value => value.Value is byte width ? width : (byte)0)];
        int[] flat = [.. data.ConstructorArguments[3].Values.Select(value => value.Value is int number ? number : 0)];

        var parts = new List<PartModel>();
        for (int i = 0; i + 2 < flat.Length; i += 3)
            parts.Add(new PartModel(flat[i], flat[i + 1], flat[i + 2]));

        return new LayoutModel(isFixed, isComplete, new EquatableArray<byte>(widths), new EquatableArray<PartModel>([.. parts]));
    }

    private static MessageModel? BuildMessage(AttributeData attribute, string typeName, Location typeLocation, List<DiagnosticInfo> diagnostics)
    {
        TypedConstant opcodeArgument = ArgumentOf(attribute, 0);
        TypedConstant directionArgument = ArgumentOf(attribute, 1);
        if (!TryGetWireValue(opcodeArgument, out ulong opcode) || opcode > ushort.MaxValue)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "the opcode must be a GameMessageOpcode value from 0 to 0xFFFF"));
            return null;
        }

        int direction = directionArgument.Value is int value ? value : 0;
        if (direction is < 1 or > 3)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadType, typeLocation, typeName, "the direction must be ClientToServer, ServerToClient or Both"));
            return null;
        }

        string opcodeName = opcodeArgument.Type?.GetMembers().OfType<IFieldSymbol>()
            .FirstOrDefault(field => field.HasConstantValue && Equals(field.ConstantValue, opcodeArgument.Value))?.Name
            ?? opcode.ToString("X4", CultureInfo.InvariantCulture);
        return new MessageModel((ushort)opcode, opcodeName, direction);
    }

    private static List<ISymbol> WireMembersOf(INamedTypeSymbol type)
    {
        var symbols = new List<ISymbol>();
        foreach (ISymbol member in type.GetMembers())
        {
            if (member.IsStatic || member.IsImplicitlyDeclared || !member.Locations.Any(location => location.IsInSource))
                continue;
            if (member is not IPropertySymbol { IsIndexer: false, ExplicitInterfaceImplementations.Length: 0 } and not IFieldSymbol { IsConst: false })
                continue;
            if (FindAttribute(member.GetAttributes(), WireNames.NotOnWire) is not null)
                continue;
            symbols.Add(member);
        }

        symbols.Sort(static (left, right) =>
        {
            Location leftLocation = left.Locations[0];
            Location rightLocation = right.Locations[0];
            int byFile = string.CompareOrdinal(leftLocation.SourceTree?.FilePath, rightLocation.SourceTree?.FilePath);
            return byFile != 0 ? byFile : leftLocation.SourceSpan.Start.CompareTo(rightLocation.SourceSpan.Start);
        });
        return symbols;
    }

    private static int DeclarationsHolding(List<ISymbol> members, CancellationToken cancellationToken)
    {
        var declarations = new HashSet<(string?, int)>();
        foreach (ISymbol member in members)
        {
            SyntaxReference? reference = member.DeclaringSyntaxReferences.FirstOrDefault();
            TypeDeclarationSyntax? declaration = reference?.GetSyntax(cancellationToken).AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
            if (declaration is not null)
                declarations.Add((declaration.SyntaxTree.FilePath, declaration.SpanStart));
        }

        return declarations.Count;
    }

    private static MemberModel? BuildMember(ISymbol symbol, string typeName, bool isMessage, List<DiagnosticInfo> diagnostics)
    {
        ITypeSymbol memberType = symbol is IPropertySymbol property ? property.Type : ((IFieldSymbol)symbol).Type;
        bool settable = symbol is IPropertySymbol { SetMethod: { IsInitOnly: false } } || symbol is IFieldSymbol { IsReadOnly: false };
        ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
        Location? location = symbol.Locations.FirstOrDefault();
        var locationInfo = LocationInfo.From(location);
        string name = symbol.Name;

        void Report(string problem)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadAttribute, location, name, typeName, problem));
        }

        if (FindAttribute(attributes, WireNames.Pad) is { } pad)
        {
            int bits = ArgumentOf(pad, 0).Value is int padBits ? padBits : 0;
            if (memberType.ToDisplayString() != WireNames.PadType)
            {
                Report("a [Pad] member must have the type Pad");
                return null;
            }

            if (bits < 1)
            {
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadWidth, location, name, typeName, Text(bits), "Pad", "a positive number of bits"));
                return null;
            }

            return Member(name, MemberShape.Pad, locationInfo, settable) with { PadBits = bits };
        }

        if (FindAttribute(attributes, WireNames.Blob) is { } blob)
        {
            int lengthBits = ArgumentOf(blob, 0).Value is int blobBits ? blobBits : 0;
            bool align = NamedBool(blob, "Align");
            bool includesSelf = NamedBool(blob, "LengthIncludesSelf");
            if (memberType is not IArrayTypeSymbol { ElementType.SpecialType: SpecialType.System_Byte, Rank: 1 })
            {
                Report("a [Blob] member must have the type byte[]");
                return null;
            }

            if (lengthBits is < 1 or > 32)
            {
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadWidth, location, name, typeName, Text(lengthBits), "byte[] length", "32"));
                return null;
            }

            if (includesSelf && lengthBits % 8 != 0)
            {
                Report("a length that counts itself must be a whole number of bytes");
                return null;
            }

            if (align && !isMessage)
            {
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.AlignedBlobInStruct, location, name, typeName));
                return null;
            }

            if (!settable)
            {
                Report("a [Blob] member needs a setter");
                return null;
            }

            return Member(name, MemberShape.Blob, locationInfo, settable) with { BlobLengthBits = lengthBits, BlobAlign = align, BlobLengthIncludesSelf = includesSelf };
        }

        if (FindAttribute(attributes, WireNames.Union) is { } union)
        {
            string? selector = ArgumentOf(union, 0).Value as string;
            bool interfaceOfStructs = memberType.TypeKind == TypeKind.Interface
                && memberType.AllInterfaces.Any(implemented => implemented.ToDisplayString() == WireNames.IWireStruct);
            bool abstractStruct = memberType.TypeKind == TypeKind.Class && memberType.IsAbstract && HasAttribute(memberType, WireNames.WireStruct);
            if (!interfaceOfStructs && !abstractStruct)
            {
                Report("a [Union] member's type must be an interface that extends IWireStruct, or an abstract [WireStruct] class");
                return null;
            }

            if (!settable)
            {
                Report("a [Union] member needs a setter");
                return null;
            }

            return Member(name, MemberShape.Union, locationInfo, settable) with { UnionType = memberType.ToDisplayString(s_fullName), UnionSelector = selector };
        }

        if (FindAttribute(attributes, WireNames.Array) is { } array)
        {
            int countBits = NamedInt(array, "CountBits");
            string? countFrom = NamedString(array, "CountFrom");
            if ((countBits > 0) == (countFrom is not null))
            {
                Report("an [Array] sets exactly one of CountBits and CountFrom");
                return null;
            }

            if (countFrom is null && countBits > 32)
            {
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadWidth, location, name, typeName, Text(countBits), "count", "32"));
                return null;
            }

            if (!TryGetCollection(memberType, out CollectionKind collection, out ITypeSymbol? elementType))
            {
                Report("an [Array] member must be a List<T> or a T[]");
                return null;
            }

            if (collection == CollectionKind.Array && !settable)
            {
                Report("an [Array] member of type T[] needs a setter");
                return null;
            }

            ValueModel? element = BuildValue(name, typeName, attributes, elementType, location, diagnostics, out bool reported);
            if (element is null)
            {
                if (!reported)
                    diagnostics.Add(DiagnosticInfo.Create(Diagnostics.MissingWidth, location, name, typeName));
                return null;
            }

            return Member(name, MemberShape.Array, locationInfo, settable) with
            {
                Value = element,
                CountBits = countBits,
                CountFrom = countFrom,
                Collection = collection,
                ElementType = elementType.ToDisplayString(s_fullName),
            };
        }

        if (FindAttribute(attributes, WireNames.FixedArray) is { } fixedArray)
        {
            int length = ArgumentOf(fixedArray, 0).Value is int fixedLength ? fixedLength : 0;
            if (memberType is not IArrayTypeSymbol { Rank: 1 } arrayType)
            {
                Report("a [FixedArray] member must be a T[]");
                return null;
            }

            if (length < 1)
            {
                Report("a [FixedArray] needs a length of at least 1");
                return null;
            }

            ValueModel? element = BuildValue(name, typeName, attributes, arrayType.ElementType, location, diagnostics, out bool reported);
            if (element is null)
            {
                if (!reported)
                    diagnostics.Add(DiagnosticInfo.Create(Diagnostics.MissingWidth, location, name, typeName));
                return null;
            }

            return Member(name, MemberShape.FixedArray, locationInfo, settable) with
            {
                Value = element,
                Collection = CollectionKind.Array,
                ElementType = arrayType.ElementType.ToDisplayString(s_fullName),
                FixedLength = length,
            };
        }

        ValueModel? value = BuildValue(name, typeName, attributes, memberType, location, diagnostics, out bool valueReported);
        if (value is null)
        {
            if (!valueReported)
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.MissingWidth, location, name, typeName));
            return null;
        }

        bool isCount = FindAttribute(attributes, WireNames.Count) is not null;
        if (isCount && (value.Scalar is not { IsBool: false, IsEnum: false, IntegerType: not null } || !settable))
        {
            Report("a [Count] member must be a settable integer with [UInt] or [SInt]");
            return null;
        }

        // The generated reader holds a count in an int, as it does a CountBits count.
        if (isCount && value.Scalar is { Bits: > 32 } wideCount)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadWidth, location, name, typeName, Text(wideCount.Bits), "count", "32"));
            return null;
        }

        if (value.Kind == ValueKind.Struct && !settable && memberType.IsReferenceType is false)
        {
            Report("a [WireStruct] member must be a class");
            return null;
        }

        if (value.Kind != ValueKind.Struct && !settable)
        {
            Report("a scalar or string member needs a setter the generated Read can assign; mark a computed member [NotOnWire]");
            return null;
        }

        return Member(name, MemberShape.Value, locationInfo, settable) with { Value = value, IsCount = isCount };
    }

    private static ValueModel? BuildValue(string name, string typeName, ImmutableArray<AttributeData> attributes, ITypeSymbol type, Location? location, List<DiagnosticInfo> diagnostics, out bool reported)
    {
        reported = false;
        var scalars = s_scalarAttributes.Select(metadataName => FindAttribute(attributes, metadataName)).Where(attribute => attribute is not null).ToList();
        var strings = s_stringAttributes.Select(metadataName => FindAttribute(attributes, metadataName)).Where(attribute => attribute is not null).ToList();

        void Report(string problem)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadAttribute, location, name, typeName, problem));
        }

        if (scalars.Count + strings.Count > 1)
        {
            Report("a member takes one encoding attribute");
            reported = true;
            return null;
        }

        string typeDisplay = type.ToDisplayString(s_fullName);
        if (scalars.Count == 1 && scalars[0] is { AttributeClass: { } scalarClass } scalar)
        {
            string attributeName = scalarClass.ToDisplayString();
            if (attributeName is WireNames.UInt or WireNames.SInt)
            {
                ScalarEncoding encoding = attributeName == WireNames.UInt ? ScalarEncoding.UInt : ScalarEncoding.SInt;
                int bits = ArgumentOf(scalar, 0).Value is int integerBits ? integerBits : 0;
                ScalarModel? integer = BuildInteger(name, typeName, encoding, bits, type, location, diagnostics);
                reported = integer is null;
                return integer is null ? null : new ValueModel(ValueKind.Scalar, integer, StringEncoding.None, null, null);
            }

            int components = typeDisplay switch
            {
                WireNames.Quaternion => 4,
                WireNames.Vector3 => 3,
                WireNames.Vector2 => 2,
                _ => 1,
            };
            (ScalarEncoding Encoding, int Bits, bool Allowed) floating = attributeName switch
            {
                WireNames.Float32 => (ScalarEncoding.Float32, 32, type.SpecialType == SpecialType.System_Single || components > 1),
                WireNames.Float64 => (ScalarEncoding.Float64, 64, type.SpecialType == SpecialType.System_Double),
                _ => (ScalarEncoding.PackedFloat, 16, type.SpecialType == SpecialType.System_Single || components > 1),
            };
            if (!floating.Allowed)
            {
                Report($"[{attributeName.Substring(WireNames.Codec.Length + 1).Replace("Attribute", "")}] does not suit the type {typeDisplay}");
                reported = true;
                return null;
            }

            var model = new ScalarModel(floating.Encoding, floating.Bits, typeDisplay, null, null, false, false, false, components, floating.Bits);
            return new ValueModel(ValueKind.Scalar, model, StringEncoding.None, null, null);
        }

        if (strings.Count == 1 && strings[0] is { AttributeClass: { } stringClass })
        {
            if (type.SpecialType != SpecialType.System_String)
            {
                Report("a string attribute needs the type string");
                reported = true;
                return null;
            }

            StringEncoding encoding = stringClass.ToDisplayString() switch
            {
                WireNames.WideString => StringEncoding.Wide,
                WireNames.NarrowString => StringEncoding.Narrow,
                _ => StringEncoding.Terminated,
            };
            return new ValueModel(ValueKind.String, null, encoding, null, null);
        }

        if (type is INamedTypeSymbol named && (HasAttribute(named, WireNames.WireStruct) || HasAttribute(named, WireNames.WireMessage)))
        {
            if (named.IsAbstract)
            {
                Report("a [WireStruct] member's type cannot be abstract; hold an abstract type as a [Union]");
                reported = true;
                return null;
            }

            // An aligned blob counts on starting at a byte boundary, which a message nested at any bit offset does not.
            if (HoldsAlignedBlob(named))
            {
                Report($"{named.Name} holds an aligned blob, so it cannot be nested; only a message at the start of a body can align");
                reported = true;
                return null;
            }

            LayoutModel? metadataLayout = named.Locations.Any(l => l.IsInSource) ? null : ReadLayoutData(named);
            return new ValueModel(ValueKind.Struct, null, StringEncoding.None, typeDisplay, metadataLayout);
        }

        return null;
    }

    private static ScalarModel? BuildInteger(string name, string typeName, ScalarEncoding encoding, int bits, ITypeSymbol type, Location? location, List<DiagnosticInfo> diagnostics)
    {
        string typeDisplay = type.ToDisplayString(s_fullName);
        if (type.SpecialType == SpecialType.System_Boolean)
        {
            if (encoding != ScalarEncoding.UInt || bits != 1)
            {
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadWidth, location, name, typeName, Text(bits), "bool", "1, as [UInt(1)]"));
                return null;
            }

            return new ScalarModel(encoding, 1, typeDisplay, null, null, false, false, true, 1, 1);
        }

        INamedTypeSymbol? enumType = type is INamedTypeSymbol { TypeKind: TypeKind.Enum } named ? named : null;
        ITypeSymbol integral = enumType?.EnumUnderlyingType ?? type;
        (int Size, bool Signed, string Keyword, string Unsigned)? shape = integral.SpecialType switch
        {
            SpecialType.System_Byte => (8, false, "byte", "byte"),
            SpecialType.System_SByte => (8, true, "sbyte", "byte"),
            SpecialType.System_UInt16 => (16, false, "ushort", "ushort"),
            SpecialType.System_Int16 => (16, true, "short", "ushort"),
            SpecialType.System_UInt32 => (32, false, "uint", "uint"),
            SpecialType.System_Int32 => (32, true, "int", "uint"),
            SpecialType.System_UInt64 => (64, false, "ulong", "ulong"),
            SpecialType.System_Int64 => (64, true, "long", "ulong"),
            _ => null,
        };
        if (shape is not { } integer)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadAttribute, location, name, typeName, $"[UInt] and [SInt] need an integer, bool or enum type, not {typeDisplay}"));
            return null;
        }

        if (bits < 1 || bits > integer.Size)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadWidth, location, name, typeName, Text(bits), typeDisplay, Text(integer.Size)));
            return null;
        }

        if (encoding == ScalarEncoding.SInt && !integer.Signed)
        {
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadAttribute, location, name, typeName, $"[SInt] needs a signed type, not {typeDisplay}"));
            return null;
        }

        // An enum is exempt: NTC003 below reports a member that does not fit the unsigned width, and writing one throws.
        if (encoding == ScalarEncoding.UInt && integer.Signed && bits < integer.Size && enumType is null)
            diagnostics.Add(DiagnosticInfo.Create(Diagnostics.NarrowSignedUInt, location, name, typeName, integer.Keyword, Text(bits)));

        // NTC003 is a warning: the member stays in the layout, and writing a value that does not fit throws.
        if (enumType is not null)
        {
            IFieldSymbol? tooWide = enumType.GetMembers().OfType<IFieldSymbol>()
                .FirstOrDefault(field => field.HasConstantValue && field.ConstantValue is not null && !Fits(field.ConstantValue, encoding, bits));
            if (tooWide is not null)
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.ValueTooWide, location, name, typeName, Text(bits), enumType.Name + "." + tooWide.Name, Convert.ToString(tooWide.ConstantValue, CultureInfo.InvariantCulture) ?? ""));
        }

        return new ScalarModel(encoding, bits, typeDisplay, integer.Keyword, integer.Unsigned, integer.Signed, enumType is not null, false, 1, integer.Size);
    }

    private static void CheckReferences(List<MemberModel> members, string typeName, List<DiagnosticInfo> diagnostics)
    {
        var selectors = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int i = 0; i < members.Count; i++)
        {
            MemberModel member = members[i];
            string? target = member.Shape switch
            {
                MemberShape.Array => member.CountFrom,
                MemberShape.Union => member.UnionSelector,
                _ => null,
            };
            if (target is null)
                continue;

            int targetIndex = members.FindIndex(candidate => candidate.Name == target);
            string? problem = null;
            if (targetIndex < 0)
                problem = "is not a wire member of this type";
            else if (targetIndex > i)
                problem = "is declared after it";
            else if (member.Shape == MemberShape.Array && !members[targetIndex].IsCount)
                problem = "is not a [Count] member";
            else if (member.Shape == MemberShape.Union && (members[targetIndex] is not { Shape: MemberShape.Value, Settable: true, Value.Scalar: { IsBool: false, IntegerType: not null } }))
                problem = "is not a settable integer or enum member with [UInt] or [SInt]";
            else if (member.Shape == MemberShape.Union && selectors.TryGetValue(target, out string? other))
                problem = $"already selects '{other}', and Write sets a selector from one union's arm";

            if (member.Shape == MemberShape.Union)
                selectors[target] = member.Name;

            if (problem is not null)
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadReference, member.Location, member.Name, typeName, target, problem));
        }

        foreach (MemberModel count in members.Where(member => member.IsCount))
        {
            if (!members.Any(member => member.CountFrom == count.Name))
                diagnostics.Add(DiagnosticInfo.Create(Diagnostics.BadAttribute, count.Location, count.Name, typeName, "no [Array] names this [Count] member"));
        }
    }

    private static MemberModel Member(string name, MemberShape shape, LocationInfo? location, bool settable)
    {
        return new MemberModel(name, shape, null, 0, null, false, CollectionKind.None, null, 0, null, null, 0, false, false, 0, settable, location);
    }

    private static bool TryGetCollection(ITypeSymbol type, out CollectionKind collection, out ITypeSymbol elementType)
    {
        if (type is IArrayTypeSymbol { Rank: 1 } arrayType)
        {
            collection = CollectionKind.Array;
            elementType = arrayType.ElementType;
            return true;
        }

        if (type is INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 } named
            && named.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.List<T>")
        {
            collection = CollectionKind.List;
            elementType = named.TypeArguments[0];
            return true;
        }

        collection = CollectionKind.None;
        elementType = type;
        return false;
    }

    private static bool Fits(object value, ScalarEncoding encoding, int bits)
    {
        if (bits == 64)
            return true;

        if (encoding == ScalarEncoding.SInt)
        {
            long signed = Convert.ToInt64(value, CultureInfo.InvariantCulture);
            long limit = 1L << (bits - 1);
            return signed >= -limit && signed < limit;
        }

        return TryGetWireValue(value, out ulong wire) && (wire >> bits) == 0;
    }

    private static bool TryGetWireValue(TypedConstant constant, out ulong value)
    {
        value = 0;
        return constant.Kind != TypedConstantKind.Array && constant.Value is not null && TryGetWireValue(constant.Value, out value);
    }

    /// <summary>
    /// The value as the wire carries it: a signed value reinterpreted through the unsigned type of its size.
    /// </summary>
    private static bool TryGetWireValue(object value, out ulong wire)
    {
        switch (value)
        {
            case byte number:
                wire = number;
                return true;
            case sbyte number:
                wire = unchecked((byte)number);
                return true;
            case ushort number:
                wire = number;
                return true;
            case short number:
                wire = unchecked((ushort)number);
                return true;
            case uint number:
                wire = number;
                return true;
            case int number:
                wire = unchecked((uint)number);
                return true;
            case ulong number:
                wire = number;
                return true;
            case long number:
                wire = unchecked((ulong)number);
                return true;
            default:
                wire = 0;
                return false;
        }
    }

    /// <summary>
    /// A [UnionCase] value sign-extended to 64 bits, so -1 is the same number whichever integer type wrote it.
    /// </summary>
    private static bool TryGetCaseValue(TypedConstant constant, out ulong value)
    {
        value = 0;
        if (constant.Kind == TypedConstantKind.Array)
            return false;

        switch (constant.Value)
        {
            case sbyte number:
                value = unchecked((ulong)number);
                return true;
            case short number:
                value = unchecked((ulong)number);
                return true;
            case int number:
                value = unchecked((ulong)number);
                return true;
            case long number:
                value = unchecked((ulong)number);
                return true;
            case byte number:
                value = number;
                return true;
            case ushort number:
                value = number;
                return true;
            case uint number:
                value = number;
                return true;
            case ulong number:
                value = number;
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// The constructor argument at <paramref name="index"/>, or an empty constant when the attribute is malformed and the compiler has already reported it.
    /// </summary>
    private static TypedConstant ArgumentOf(AttributeData attribute, int index)
    {
        return index < attribute.ConstructorArguments.Length ? attribute.ConstructorArguments[index] : default;
    }

    private static bool HoldsAlignedBlob(INamedTypeSymbol type)
    {
        for (INamedTypeSymbol? current = type; current is not null; current = current.BaseType)
        {
            foreach (ISymbol member in current.GetMembers())
            {
                if (FindAttribute(member.GetAttributes(), WireNames.Blob) is { } blob && NamedBool(blob, "Align"))
                    return true;
            }
        }

        return false;
    }

    private static bool IsPartial(INamedTypeSymbol type, CancellationToken cancellationToken)
    {
        foreach (SyntaxReference reference in type.DeclaringSyntaxReferences)
        {
            if (reference.GetSyntax(cancellationToken) is TypeDeclarationSyntax declaration && !declaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                return false;
        }

        return true;
    }

    private static string KeywordOf(INamedTypeSymbol type)
    {
        if (type.IsRecord)
            return type.TypeKind == TypeKind.Struct ? "record struct" : "record";
        return type.TypeKind == TypeKind.Struct ? "struct" : "class";
    }

    public static bool HasAttribute(ISymbol symbol, string metadataName)
    {
        return FindAttribute(symbol.GetAttributes(), metadataName) is not null;
    }

    private static AttributeData? FindAttribute(ImmutableArray<AttributeData> attributes, string metadataName)
    {
        foreach (AttributeData attribute in attributes)
        {
            if (attribute.AttributeClass?.ToDisplayString() == metadataName)
                return attribute;
        }

        return null;
    }

    private static bool NamedBool(AttributeData attribute, string name)
    {
        return attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value is true;
    }

    private static int NamedInt(AttributeData attribute, string name)
    {
        return attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value is int value ? value : 0;
    }

    private static string? NamedString(AttributeData attribute, string name)
    {
        return attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as string;
    }

    private static string Text(int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
