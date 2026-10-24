using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// The generator's diagnostics. Each error stops the type's code from being emitted, so the build fails at the declaration rather than in generated code.
/// </summary>
internal static class Diagnostics
{
    private const string Category = "NexusTomorrow.Codec";

    public static readonly DiagnosticDescriptor MissingWidth = new(
        "NTC001", "Wire member has no width",
        "Member '{0}' of '{1}' has no wire width; add [UInt(n)], [SInt(n)], [Float32], [Float64], [PackedFloat], a string, array, union, blob or pad attribute, give it a [WireStruct] or [WireMessage] type, or mark it [NotOnWire]",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "A wire member has no encoding, isn't a `[WireStruct]` or `[WireMessage]` type and isn't `[NotOnWire]`");

    public static readonly DiagnosticDescriptor BadWidth = new(
        "NTC002", "Width does not suit the member's type",
        "Member '{0}' of '{1}' declares {2} bits, which its type {3} cannot hold or which is outside 1 to {4}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "A width the member's type can't hold, or outside its range: 1 to 64 bits, a `bool` only `[UInt(1)]`, a count or a blob length 1 to 32, a `[Pad]` of at least 1");

    /// <summary>
    /// A warning, so a sentinel member that is never sent can be suppressed deliberately; the generated Write throws if one is.
    /// </summary>
    public static readonly DiagnosticDescriptor ValueTooWide = new(
        "NTC003", "Enum value does not fit the declared width",
        "Member '{0}' of '{1}' is {2} bits wide, but {3} = {4} needs more; writing it throws",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
        description: "An enum member needs more bits than its field declares; the member stays, so a sentinel value that's never sent can be suppressed with its reason, and writing it throws");

    public static readonly DiagnosticDescriptor BadReference = new(
        "NTC004", "Count or selector reference is wrong",
        "Member '{0}' of '{1}' names '{2}', which {3}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "`CountFrom` or a union's selector names a member that's missing, declared later, or of the wrong kind, or two unions share one selector");

    public static readonly DiagnosticDescriptor Duplicate = new(
        "NTC005", "Opcode or union case declared twice",
        "{0}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "Two messages share an opcode and direction, or two arms share a union value");

    public static readonly DiagnosticDescriptor NarrowSignedUInt = new(
        "NTC006", "Unsigned width on a signed type",
        "Member '{0}' of '{1}' is a signed {2} read as an unsigned {3}-bit value; a negative value cannot be written, so declare [SInt({3})] if the field is signed",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
        description: "`[UInt(n)]` on a signed integer narrower than its type, so a negative value can't be written; enums are exempt, as `NTC003` covers them");

    public static readonly DiagnosticDescriptor SplitAcrossParts = new(
        "NTC007", "Wire members in more than one partial declaration",
        "'{0}' declares wire members in more than one partial declaration, so their order is undefined; keep them in one",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "Wire members spread across the parts of a partial class, so their order is undefined");

    public static readonly DiagnosticDescriptor NotPartial = new(
        "NTC008", "Type is not partial",
        "'{0}' must be partial, as must every type that contains it, so the generator can implement it",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "The type, or a type containing it, isn't `partial`");

    public static readonly DiagnosticDescriptor BadAttribute = new(
        "NTC009", "Wire attribute misused",
        "Member '{0}' of '{1}': {2}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "An attribute misused: both or neither of `CountBits` and `CountFrom`; a second scalar or string encoding on one member; an encoding that doesn't suit the member's type (a float or string attribute on the wrong type, `[SInt]` on an unsigned type, `[UInt]` or `[SInt]` on a type that isn't an integer, bool or enum, `[Blob]` off `byte[]`, `[Pad]` off `Pad`, `[Array]` off a `List<T>` or `T[]`, `[FixedArray]` off `T[]`); a `[FixedArray]` length under 1; a self-counting blob length that isn't whole bytes; a union type that isn't a wire interface or abstract `[WireStruct]`; an abstract `[WireStruct]` held as a plain member or as an array element; a nested type holding an aligned blob; a `[WireStruct]` value type with no setter; a `[Count]` that isn't a settable non-enum integer or that no array names; a `[Blob]`, `[Union]`, `[Array]` of `T[]`, scalar or string member with no setter or only an `init` one (a get-only `List<T>`, `[FixedArray]` or `[WireStruct]` class member is filled in place)");

    public static readonly DiagnosticDescriptor NoArms = new(
        "NTC010", "Union has no arms",
        "Member '{0}' of '{1}' is a union of {2}, but no [WireStruct, UnionCase] type in this assembly implements it",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "A union type with no arm in the assembly");

    public static readonly DiagnosticDescriptor AlignedBlobInStruct = new(
        "NTC011", "Aligned blob outside a message",
        "Member '{0}' of '{1}' is an aligned blob; only a [WireMessage] type, which starts on a byte boundary, can align",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "An aligned blob outside a `[WireMessage]`, the only kind of type that starts on a byte boundary");

    public static readonly DiagnosticDescriptor BadType = new(
        "NTC012", "Wire type misdeclared",
        "'{0}': {1}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "The type itself is misdeclared: a record, generic or nested in a generic type, both `[WireStruct]` and `[WireMessage]`, an abstract message, a non-abstract type with no public or internal parameterless constructor, a bad `[UnionCase]` value, opcode or direction, or a union arm holding an aligned blob");

    public static readonly DiagnosticDescriptor CaseTooWide = new(
        "NTC013", "Union case does not fit its selector",
        "Member '{0}' of '{1}' is {2} bits wide, but the [UnionCase] of {3} = {4} needs more, so that arm could never be read back",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "A `[UnionCase]` value its selector can't carry, so that arm could never be read back");

    public static readonly DiagnosticDescriptor CapacityOverCount = new(
        "NTC014", "Capacity does not fit its count",
        "Member '{0}' of '{1}' declares a capacity of {2}, but its {3}-bit count holds at most {4}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true,
        description: "An `[Array]` `Capacity` larger than its own count's width can carry, so the bound could never be reached");

    /// <summary>
    /// Every descriptor, in id order, for the reference page.
    /// </summary>
    public static IReadOnlyList<DiagnosticDescriptor> All { get; } =
    [
        MissingWidth, BadWidth, ValueTooWide, BadReference, Duplicate, NarrowSignedUInt, SplitAcrossParts,
        NotPartial, BadAttribute, NoArms, AlignedBlobInStruct, BadType, CaseTooWide, CapacityOverCount,
    ];

    public static DiagnosticDescriptor ById(string id)
    {
        foreach (DiagnosticDescriptor descriptor in All)
        {
            if (descriptor.Id == id)
                return descriptor;
        }

        throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown diagnostic id.");
    }
}

/// <summary>
/// A source location held as values, so models that carry it stay equatable.
/// </summary>
internal sealed record LocationInfo(string FilePath, TextSpan Span, LinePositionSpan LineSpan)
{
    public static LocationInfo? From(Location? location)
    {
        if (location is null || location.SourceTree is null)
            return null;

        return new LocationInfo(location.SourceTree.FilePath, location.SourceSpan, location.GetLineSpan().Span);
    }

    public Location ToLocation()
    {
        return Location.Create(FilePath, Span, LineSpan);
    }
}

/// <summary>
/// A diagnostic held as values until the output step reports it.
/// </summary>
internal sealed record DiagnosticInfo(string Id, LocationInfo? Location, EquatableArray<string> Arguments)
{
    public static DiagnosticInfo Create(DiagnosticDescriptor descriptor, Location? location, params string[] arguments)
    {
        return new DiagnosticInfo(descriptor.Id, LocationInfo.From(location), new EquatableArray<string>(arguments));
    }

    /// <summary>
    /// Create from a location already held as values; <see cref="LocationInfo.ToLocation"/> gives a location with no syntax tree, which <see cref="LocationInfo.From"/> cannot read back.
    /// </summary>
    public static DiagnosticInfo Create(DiagnosticDescriptor descriptor, LocationInfo? location, params string[] arguments)
    {
        return new DiagnosticInfo(descriptor.Id, location, new EquatableArray<string>(arguments));
    }

    public bool IsError => Diagnostics.ById(Id).DefaultSeverity == DiagnosticSeverity.Error;

    /// <summary>
    /// The diagnostic on the syntax tree its file path names, so path-scoped <c>.editorconfig</c> severities and <c>#pragma</c> reach it,
    /// or at the bare file path when <paramref name="trees"/> has no tree for it.
    /// </summary>
    public Diagnostic ToDiagnostic(IReadOnlyDictionary<string, SyntaxTree> trees)
    {
        string[] arguments = new string[Arguments.Count];
        for (int i = 0; i < arguments.Length; i++)
            arguments[i] = Arguments[i];

        Location? location = Location is { } info
            ? (trees.TryGetValue(info.FilePath, out SyntaxTree? tree) ? Microsoft.CodeAnalysis.Location.Create(tree, info.Span) : info.ToLocation())
            : null;
        return Diagnostic.Create(Diagnostics.ById(Id), location, arguments);
    }
}
