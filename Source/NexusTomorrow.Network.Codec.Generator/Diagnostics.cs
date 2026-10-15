using System;
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
        "Member '{0}' of '{1}' has no wire width; add [UInt(n)], [SInt(n)], [Float32], [Float64], [PackedFloat], a string, array, union, blob or pad attribute, give it a [WireStruct] type, or mark it [NotOnWire]",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor BadWidth = new(
        "NTC002", "Width does not suit the member's type",
        "Member '{0}' of '{1}' declares {2} bits, which its type {3} cannot hold or which is outside 1 to {4}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ValueTooWide = new(
        "NTC003", "Value does not fit the declared width",
        "Member '{0}' of '{1}' is {2} bits wide, but {3} = {4} needs more",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor BadReference = new(
        "NTC004", "Count or selector reference is wrong",
        "Member '{0}' of '{1}' names '{2}', which {3}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor Duplicate = new(
        "NTC005", "Opcode or union case declared twice",
        "{0}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NarrowSignedUInt = new(
        "NTC006", "Unsigned width on a signed type",
        "Member '{0}' of '{1}' is a signed {2} read as an unsigned {3}-bit value; a negative value cannot be written, so declare [SInt({3})] if the field is signed",
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor SplitAcrossParts = new(
        "NTC007", "Wire members in more than one partial declaration",
        "'{0}' declares wire members in more than one partial declaration, so their order is undefined; keep them in one",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NotPartial = new(
        "NTC008", "Type is not partial",
        "'{0}' must be partial, as must every type that contains it, so the generator can implement it",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor BadAttribute = new(
        "NTC009", "Wire attribute misused",
        "Member '{0}' of '{1}': {2}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NoArms = new(
        "NTC010", "Union has no arms",
        "Member '{0}' of '{1}' is a union of {2}, but no [WireStruct, UnionCase] type in this assembly implements it",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor AlignedBlobInStruct = new(
        "NTC011", "Aligned blob outside a message",
        "Member '{0}' of '{1}' is an aligned blob; only a [WireMessage] type, which starts on a byte boundary, can align",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor BadType = new(
        "NTC012", "Wire type misdeclared",
        "'{0}': {1}",
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static DiagnosticDescriptor ById(string id)
    {
        return id switch
        {
            "NTC001" => MissingWidth,
            "NTC002" => BadWidth,
            "NTC003" => ValueTooWide,
            "NTC004" => BadReference,
            "NTC005" => Duplicate,
            "NTC006" => NarrowSignedUInt,
            "NTC007" => SplitAcrossParts,
            "NTC008" => NotPartial,
            "NTC009" => BadAttribute,
            "NTC010" => NoArms,
            "NTC011" => AlignedBlobInStruct,
            "NTC012" => BadType,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown diagnostic id."),
        };
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

    public Diagnostic ToDiagnostic()
    {
        string[] arguments = new string[Arguments.Count];
        for (int i = 0; i < arguments.Length; i++)
            arguments[i] = Arguments[i];
        return Diagnostic.Create(Diagnostics.ById(Id), Location?.ToLocation(), arguments);
    }
}
