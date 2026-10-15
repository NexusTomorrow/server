using Microsoft.CodeAnalysis;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// What one statement of a legacy <c>Read</c> or <c>Write</c> body does to the wire, before counts are matched to their arrays.
/// </summary>
internal abstract record RawOp;

/// <summary>
/// A scalar member at an encoding ("UInt", "Float32", "Float64" or "PackedFloat") and a width in bits.
/// </summary>
internal sealed record ScalarOp(ISymbol Member, string Encoding, int Bits) : RawOp;

/// <summary>
/// A string member under one of the string attributes.
/// </summary>
internal sealed record StringOp(ISymbol Member, string Attribute) : RawOp;

/// <summary>
/// A member whose own model type reads or writes itself.
/// </summary>
internal sealed record StructOp(ISymbol Member) : RawOp;

/// <summary>
/// A count: on the write side the length of <see cref="List"/>, on the read side the local <see cref="Local"/>.
/// </summary>
internal sealed record CountOp(ISymbol? List, string? Local, int Bits) : RawOp;

/// <summary>
/// The elements of a list or array member, counted by the count named <see cref="CountLocal"/> (read side),
/// the latest count (write side), or a fixed length.
/// </summary>
internal sealed record LoopOp(ISymbol List, ElementOp Element, string? CountLocal, int? FixedLength) : RawOp;

/// <summary>
/// The bytes of a <c>byte[]</c> member: a fixed length, or a length the count before it carries.
/// </summary>
internal sealed record BytesOp(ISymbol Member, int? FixedLength, string? CountLocal) : RawOp;

/// <summary>
/// Zero bits on the wire: a constant zero written, or bits read and discarded.
/// </summary>
internal sealed record PadOp(int Bits) : RawOp;

/// <summary>
/// A call to the base type's own body, which the declaration's base layout replaces.
/// </summary>
internal sealed record BaseOp : RawOp;

/// <summary>
/// One element of a list: a scalar (<see cref="Encoding"/> and <see cref="Bits"/>), a string (<see cref="StringAttribute"/>),
/// or a model type that reads or writes itself (neither set).
/// </summary>
internal sealed record ElementOp(string? Encoding, int Bits, string? StringAttribute);
