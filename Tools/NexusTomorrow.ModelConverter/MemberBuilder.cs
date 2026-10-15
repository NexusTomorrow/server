using Microsoft.CodeAnalysis;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// Turns one body's raw operations into the declaration's wire members: a count directly before its one list becomes
/// <c>[Array(CountBits = n)]</c>, a count shared by several lists (or separated from its list) becomes an added
/// <c>[Count]</c> member the lists name, and a member written wider than its type becomes its type's width plus a pad.
/// </summary>
internal sealed class MemberBuilder
{
    private static readonly WireMember s_removed = new("", "", null);

    private readonly List<WireMember?> _members = [];
    private readonly List<CountGroup> _groups = [];
    private CountGroup? _latest;
    private int _pads;
    private string? _failure;

    private MemberBuilder()
    {
    }

    /// <summary>
    /// Build the members, or return the reason the body cannot be declared.
    /// </summary>
    public static (List<WireMember> Members, string? Failure) Build(IReadOnlyList<RawOp> ops)
    {
        var builder = new MemberBuilder();
        for (int i = 0; i < ops.Count && builder._failure is null; i++)
            builder.Add(ops[i], i);
        if (builder._failure is null)
            builder.ResolveCounts();

        return builder._failure is null
            ? ([.. builder._members.Where(member => member is not null && !ReferenceEquals(member, s_removed)).Select(member => member!)], null)
            : ([], builder._failure);
    }

    private void Add(RawOp op, int index)
    {
        switch (op)
        {
            case BaseOp when index != 0:
                Fail("the base type's body is not called first");
                break;
            case BaseOp:
                break;
            case ScalarOp scalar:
                {
                    if (!TryScalar(TypeOf(scalar.Member), scalar.Encoding, scalar.Bits, out string attribute, out int padBits))
                    {
                        Fail($"{scalar.Member.Name} is {scalar.Encoding} at {scalar.Bits} bits, which its type {TypeOf(scalar.Member)} cannot declare");
                        break;
                    }

                    _members.Add(new WireMember(scalar.Member.Name, attribute, null));
                    if (padBits > 0)
                        _members.Add(new WireMember(scalar.Member.Name + "High", $"Pad({padBits})", "Pad"));
                    break;
                }

            case StringOp text when TypeOf(text.Member).SpecialType != SpecialType.System_String:
                Fail($"{text.Member.Name} is written as a string but is not one");
                break;
            case StringOp text:
                _members.Add(new WireMember(text.Member.Name, text.Attribute, null));
                break;
            case StructOp nested when TypeOf(nested.Member) is not INamedTypeSymbol { TypeKind: TypeKind.Class }:
                Fail($"{nested.Member.Name} is an interface or struct, a union for the hand pass");
                break;
            case StructOp nested:
                _members.Add(new WireMember(nested.Member.Name, "", null));
                break;
            case PadOp pad:
                _members.Add(new WireMember($"Pad{_pads++}", $"Pad({pad.Bits})", "Pad"));
                break;
            case CountOp count when count.Bits > 32:
                Fail($"a count is {count.Bits} bits wide");
                break;
            case CountOp count:
                _latest = new CountGroup { Slot = _members.Count, Bits = count.Bits, Counted = count.List, Local = count.Local };
                _groups.Add(_latest);
                _members.Add(null);
                break;
            case LoopOp loop:
                AddLoop(loop);
                break;
            case BytesOp bytes:
                AddBytes(bytes);
                break;
            default:
                Fail("an operation the converter does not know");
                break;
        }
    }

    private void AddLoop(LoopOp loop)
    {
        string? element = ElementAttribute(loop.List, loop.Element);
        if (element is null)
            return;

        if (loop.FixedLength is int length)
        {
            if (TypeOf(loop.List) is not IArrayTypeSymbol)
            {
                Fail($"{loop.List.Name} has a fixed length but is not an array");
                return;
            }

            _members.Add(new WireMember(loop.List.Name, Join($"FixedArray({length})", element), null));
            return;
        }

        CountGroup? group = loop.CountLocal is not null ? _groups.LastOrDefault(candidate => candidate.Local == loop.CountLocal) : _latest;
        if (group is null)
        {
            Fail($"{loop.List.Name} has no count before it");
            return;
        }

        group.Lists.Add((loop.List, element, _members.Count));
        _members.Add(null);
    }

    private void AddBytes(BytesOp bytes)
    {
        if (TypeOf(bytes.Member) is not IArrayTypeSymbol { ElementType.SpecialType: SpecialType.System_Byte })
        {
            Fail($"{bytes.Member.Name} is written as bytes but is not a byte[]");
            return;
        }

        if (bytes.FixedLength is int length)
        {
            _members.Add(new WireMember(bytes.Member.Name, $"FixedArray({length}), UInt(8)", null));
            return;
        }

        CountGroup? group = bytes.CountLocal is not null ? _groups.LastOrDefault(candidate => candidate.Local == bytes.CountLocal) : _latest;
        if (group is null || group.Bytes is not null)
        {
            Fail($"{bytes.Member.Name} has no length before it");
            return;
        }

        group.Bytes = (bytes.Member, _members.Count);
        _members.Add(null);
    }

    private void ResolveCounts()
    {
        foreach (CountGroup group in _groups)
        {
            if (group.Bytes is { } bytes)
            {
                if (group.Lists.Count > 0 || bytes.Slot != group.Slot + 1 || (group.Counted is not null && !SymbolEqualityComparer.Default.Equals(group.Counted, bytes.Member)))
                {
                    Fail($"the length before {bytes.Member.Name} is not its own");
                    return;
                }

                _members[group.Slot] = s_removed;
                _members[bytes.Slot] = new WireMember(bytes.Member.Name, $"Blob({group.Bits})", null);
                continue;
            }

            if (group.Lists.Count == 0)
            {
                Fail("a count has no list after it");
                return;
            }

            (ISymbol first, string firstElement, int firstSlot) = group.Lists[0];
            bool counted = group.Counted is null || SymbolEqualityComparer.Default.Equals(group.Counted, first);
            if (group.Lists.Count == 1 && firstSlot == group.Slot + 1 && counted)
            {
                _members[group.Slot] = s_removed;
                _members[firstSlot] = new WireMember(first.Name, Join($"Array(CountBits = {group.Bits})", firstElement), null);
                continue;
            }

            if (group.Counted is not null && !group.Lists.Any(list => SymbolEqualityComparer.Default.Equals(list.List, group.Counted)))
            {
                Fail($"a count is the length of {group.Counted.Name}, which is not written after it");
                return;
            }

            string countName = first.Name + "Count";
            string countType = group.Bits <= 8 ? "byte" : group.Bits <= 16 ? "ushort" : "uint";
            _members[group.Slot] = new WireMember(countName, $"UInt({group.Bits}), Count", countType);
            foreach ((ISymbol list, string element, int slot) in group.Lists)
                _members[slot] = new WireMember(list.Name, Join($"Array(CountFrom = nameof({countName}))", element), null);
        }
    }

    private string? ElementAttribute(ISymbol list, ElementOp element)
    {
        ITypeSymbol? elementType = TypeOf(list) switch
        {
            IArrayTypeSymbol array => array.ElementType,
            INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 } named when named.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.List<T>" => named.TypeArguments[0],
            _ => null,
        };
        if (elementType is null)
        {
            Fail($"{list.Name} is not a List<T> or an array");
            return null;
        }

        if (element.Encoding is not null)
        {
            if (TryScalar(elementType, element.Encoding, element.Bits, out string attribute, out int padBits) && padBits == 0)
                return attribute;

            Fail($"the elements of {list.Name} are {element.Encoding} at {element.Bits} bits, which {elementType} cannot declare");
            return null;
        }

        if (element.StringAttribute is not null)
        {
            if (elementType.SpecialType == SpecialType.System_String)
                return element.StringAttribute;

            Fail($"the elements of {list.Name} are written as strings but are not");
            return null;
        }

        if (elementType is INamedTypeSymbol { TypeKind: TypeKind.Class })
            return "";

        Fail($"the elements of {list.Name} are an interface or struct, a union for the hand pass");
        return null;
    }

    /// <summary>
    /// The attribute for a scalar of <paramref name="type"/> written at <paramref name="bits"/>; a width past the type's size
    /// declares the type's size and leaves the rest as <paramref name="padBits"/>, the zeros the legacy writer emits for a non-negative value.
    /// </summary>
    internal static bool TryScalar(ITypeSymbol type, string encoding, int bits, out string attribute, out int padBits)
    {
        attribute = "";
        padBits = 0;
        bool isVector = type.ToDisplayString() == "System.Numerics.Vector3";
        switch (encoding)
        {
            case "UInt" when type.SpecialType == SpecialType.System_Boolean:
                attribute = "UInt(1)";
                return bits == 1;
            case "UInt":
                {
                    int? size = IntegerSize(type);
                    if (size is not int typeBits || bits < 1 || bits > 64)
                        return false;

                    attribute = $"UInt({Math.Min(bits, typeBits)})";
                    padBits = Math.Max(0, bits - typeBits);
                    return true;
                }

            case "Float32":
                attribute = "Float32";
                return bits == 32 && (type.SpecialType == SpecialType.System_Single || isVector);
            case "Float64":
                attribute = "Float64";
                return bits == 64 && type.SpecialType == SpecialType.System_Double;
            case "PackedFloat":
                attribute = "PackedFloat";
                return type.SpecialType == SpecialType.System_Single || isVector;
            default:
                return false;
        }
    }

    internal static int? IntegerSize(ITypeSymbol type)
    {
        ITypeSymbol integral = type is INamedTypeSymbol { TypeKind: TypeKind.Enum, EnumUnderlyingType: { } underlying } ? underlying : type;
        return integral.SpecialType switch
        {
            SpecialType.System_Byte or SpecialType.System_SByte => 8,
            SpecialType.System_UInt16 or SpecialType.System_Int16 => 16,
            SpecialType.System_UInt32 or SpecialType.System_Int32 => 32,
            SpecialType.System_UInt64 or SpecialType.System_Int64 => 64,
            _ => null,
        };
    }

    internal static ITypeSymbol TypeOf(ISymbol member)
    {
        return member switch
        {
            IPropertySymbol property => property.Type,
            IFieldSymbol field => field.Type,
            ILocalSymbol local => local.Type,
            _ => throw new ArgumentException("Only properties, fields and locals have a member type.", nameof(member)),
        };
    }

    private static string Join(string first, string second)
    {
        return second.Length == 0 ? first : $"{first}, {second}";
    }

    private void Fail(string reason)
    {
        _failure ??= reason;
    }

    private sealed class CountGroup
    {
        public required int Slot { get; init; }

        public required int Bits { get; init; }

        public ISymbol? Counted { get; init; }

        public string? Local { get; init; }

        public List<(ISymbol List, string Element, int Slot)> Lists { get; } = [];

        public (ISymbol Member, int Slot)? Bytes { get; set; }
    }
}
