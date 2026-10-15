namespace NexusTomorrow.Network.Codec.Generator;

internal enum ScalarEncoding
{
    UInt,
    SInt,
    Float32,
    Float64,
    PackedFloat,
}

internal enum ValueKind
{
    Scalar,
    String,
    Struct,
}

internal enum StringEncoding
{
    None,
    Wide,
    Narrow,
    Terminated,
}

internal enum MemberShape
{
    Value,
    Array,
    FixedArray,
    Union,
    Blob,
    Pad,
}

internal enum CollectionKind
{
    None,
    List,
    Array,
}

/// <summary>
/// How a scalar travels: its encoding, its width, and the C# types the generated casts go through.
/// </summary>
/// <param name="ClrType">The member's type, fully qualified.</param>
/// <param name="IntegerType">For an enum, its underlying type; otherwise the integral type itself, or null for bool and floats.</param>
/// <param name="UnsignedType">The unsigned type of <paramref name="IntegerType"/>'s size, through which a signed value reaches the wire.</param>
/// <param name="IsSigned">True when <paramref name="IntegerType"/> is signed.</param>
/// <param name="Components">The floats a value carries: 1, or 2 for a Vector2 and 3 for a Vector3, each at <paramref name="Bits"/>.</param>
/// <param name="TypeBits">The size of <paramref name="IntegerType"/> in bits; for bool and floats, <paramref name="Bits"/>.</param>
internal sealed record ScalarModel(ScalarEncoding Encoding, int Bits, string ClrType, string? IntegerType, string? UnsignedType, bool IsSigned, bool IsEnum, bool IsBool, int Components, int TypeBits);

/// <summary>
/// What one value is: a scalar, a string, or a [WireStruct] type (with its layout when it comes from another assembly).
/// </summary>
internal sealed record ValueModel(ValueKind Kind, ScalarModel? Scalar, StringEncoding String, string? StructType, LayoutModel? StructMetadataLayout);

internal sealed record MemberModel(
    string Name,
    MemberShape Shape,
    ValueModel? Value,
    int CountBits,
    string? CountFrom,
    bool IsCount,
    CollectionKind Collection,
    string? ElementType,
    int FixedLength,
    string? UnionType,
    string? UnionSelector,
    int BlobLengthBits,
    bool BlobAlign,
    bool BlobLengthIncludesSelf,
    int PadBits,
    bool Settable,
    LocationInfo? Location);

internal sealed record PartModel(int Shape, int At, int ElementBits);

internal sealed record LayoutModel(bool IsFixed, bool IsComplete, EquatableArray<byte> Widths, EquatableArray<PartModel> Parts)
{
    public int Bits
    {
        get
        {
            int bits = 0;
            foreach (byte width in Widths)
                bits += width;
            return bits;
        }
    }
}

internal sealed record ContainingTypeModel(string Keyword, string Name);

/// <summary>
/// One generated source file.
/// </summary>
internal sealed record GeneratedSource(string HintName, string Text);

/// <summary>
/// Everything one generator run produces from the models: the sources and the diagnostics, held as values so the run caches.
/// </summary>
internal sealed record Production(EquatableArray<GeneratedSource> Sources, EquatableArray<DiagnosticInfo> Diagnostics);

internal sealed record MessageModel(ushort Opcode, string OpcodeExpression, int Direction);

/// <summary>
/// One [WireStruct] or [WireMessage] class as the generator sees it.
/// </summary>
/// <param name="UnionCaseValue">The [UnionCase] value sign-extended to 64 bits, so -1 is one number whatever type declared it.</param>
internal sealed record TypeModel(
    string FullName,
    string Namespace,
    string Name,
    string Keyword,
    EquatableArray<ContainingTypeModel> ContainingTypes,
    bool IsSealed,
    string? WireBase,
    LayoutModel? WireBaseMetadataLayout,
    MessageModel? Message,
    bool HasUnionCase,
    ulong UnionCaseValue,
    EquatableArray<string> Supertypes,
    EquatableArray<MemberModel> Members,
    EquatableArray<DiagnosticInfo> Diagnostics,
    LocationInfo? Location);
