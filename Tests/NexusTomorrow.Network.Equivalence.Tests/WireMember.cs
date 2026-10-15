using System.Reflection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// One wire member of a declared type. <see cref="Encoding"/>, <see cref="Bits"/> and <see cref="ValueType"/> describe the value of a value member and each element of an array member;
/// <see cref="Bits"/> is the width of a [UInt] or [SInt] value and 0 otherwise.
/// </summary>
internal sealed record WireMember(MemberInfo Member, WireShape Shape, WireEncoding Encoding, int Bits, Type ValueType)
{
    public string Name => Member.Name;

    public string QualifiedName => $"{Member.DeclaringType?.Name}.{Member.Name}";

    public Type MemberType => MemberAccess.TypeOf(Member);

    public bool IsCount { get; init; }

    public int CountBits { get; init; }

    public string? CountFrom { get; init; }

    public int FixedLength { get; init; }

    public string? Selector { get; init; }
}
