namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// The encoding of a value member, or of each element of an array member.
/// </summary>
internal enum WireEncoding
{
    None,
    UInt,
    SInt,
    Float32,
    Float64,
    PackedFloat,
    WideString,
    NarrowString,
    TerminatedWideString,
    Struct,
}
