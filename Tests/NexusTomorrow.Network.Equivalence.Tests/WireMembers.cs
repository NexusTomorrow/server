using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// The wire members of a declared type as the generator takes them: every instance property and field the type declares except [NotOnWire] ones, after its wire base type's.
/// </summary>
internal static class WireMembers
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<WireMember>> s_members = new();

    /// <summary>
    /// The union member of a holder whose only wire members are one union and its selector, or null for any other type.
    /// </summary>
    public static WireMember? SingleUnionOf(Type type)
    {
        List<WireMember> members = [.. Of(type).Where(member => member.Shape != WireShape.Pad)];
        return members is [var selector, var union] && union.Shape == WireShape.Union && union.Selector == selector.Name ? union : null;
    }

    /// <summary>
    /// Whether <paramref name="member"/> selects a union among <paramref name="members"/>; Write sets it from the arm, so a legacy twin may compute it instead of holding it.
    /// </summary>
    public static bool IsSelector(WireMember member, IReadOnlyList<WireMember> members)
    {
        return members.Any(other => other.Selector == member.Name);
    }

    /// <summary>
    /// True when <paramref name="type"/> carries [WireStruct] or [WireMessage].
    /// </summary>
    public static bool IsWireType(Type type)
    {
        return type.IsDefined(typeof(WireStructAttribute), inherit: false) || type.IsDefined(typeof(WireMessageAttribute), inherit: false);
    }

    /// <summary>
    /// The wire members of <paramref name="type"/>, its wire base type's first.
    /// </summary>
    public static IReadOnlyList<WireMember> Of(Type type)
    {
        return s_members.GetOrAdd(type, Build);
    }

    private static List<WireMember> Build(Type type)
    {
        var members = new List<WireMember>();
        if (type.BaseType is { } baseType && IsWireType(baseType))
            members.AddRange(Of(baseType));

        // Metadata keeps declaration order among properties and among fields but not between the two, so a type
        // that mixes them lists its properties first; only the order of reported differences depends on it.
        IEnumerable<MemberInfo> properties = type.GetProperties(MemberAccess.DeclaredInstance)
            .Where(property => property.GetIndexParameters().Length == 0 && !property.Name.Contains('.'))
            .OrderBy(property => property.MetadataToken);
        IEnumerable<MemberInfo> fields = type.GetFields(MemberAccess.DeclaredInstance)
            .Where(field => !field.IsDefined(typeof(CompilerGeneratedAttribute)))
            .OrderBy(field => field.MetadataToken);

        foreach (MemberInfo member in properties.Concat(fields))
        {
            if (!member.IsDefined(typeof(NotOnWireAttribute)))
                members.Add(ToWireMember(member));
        }

        return members;
    }

    private static WireMember ToWireMember(MemberInfo member)
    {
        Type type = MemberAccess.TypeOf(member);
        if (member.GetCustomAttribute<PadAttribute>() is not null)
            return new WireMember(member, WireShape.Pad, WireEncoding.None, 0, type);
        if (member.IsDefined(typeof(BlobAttribute)))
            return new WireMember(member, WireShape.Blob, WireEncoding.None, 0, type);
        if (member.GetCustomAttribute<UnionAttribute>() is { } union)
            return new WireMember(member, WireShape.Union, WireEncoding.None, 0, type) { Selector = union.Selector };

        if (member.GetCustomAttribute<ArrayAttribute>() is { } array)
        {
            Type element = CollectionAccess.ElementTypeOf(type);
            (WireEncoding encoding, int bits) = EncodingOf(member, element);
            return new WireMember(member, WireShape.Array, encoding, bits, element) { CountBits = array.CountBits, CountFrom = array.CountFrom };
        }

        if (member.GetCustomAttribute<FixedArrayAttribute>() is { } fixedArray)
        {
            Type element = CollectionAccess.ElementTypeOf(type);
            (WireEncoding encoding, int bits) = EncodingOf(member, element);
            return new WireMember(member, WireShape.FixedArray, encoding, bits, element) { FixedLength = fixedArray.Length };
        }

        (WireEncoding valueEncoding, int valueBits) = EncodingOf(member, type);
        return new WireMember(member, WireShape.Value, valueEncoding, valueBits, type) { IsCount = member.IsDefined(typeof(CountAttribute)) };
    }

    private static (WireEncoding Encoding, int Bits) EncodingOf(MemberInfo member, Type valueType)
    {
        if (member.GetCustomAttribute<UIntAttribute>() is { } unsigned)
            return (WireEncoding.UInt, unsigned.Bits);
        if (member.GetCustomAttribute<SIntAttribute>() is { } signed)
            return (WireEncoding.SInt, signed.Bits);
        if (member.IsDefined(typeof(Float32Attribute)))
            return (WireEncoding.Float32, 0);
        if (member.IsDefined(typeof(Float64Attribute)))
            return (WireEncoding.Float64, 0);
        if (member.IsDefined(typeof(PackedFloatAttribute)))
            return (WireEncoding.PackedFloat, 0);
        if (member.IsDefined(typeof(WideStringAttribute)))
            return (WireEncoding.WideString, 0);
        if (member.IsDefined(typeof(NarrowStringAttribute)))
            return (WireEncoding.NarrowString, 0);
        if (member.IsDefined(typeof(TerminatedWideStringAttribute)))
            return (WireEncoding.TerminatedWideString, 0);
        // A message nests as it is, as the client nests whole message bodies in lists.
        if (IsWireType(valueType))
            return (WireEncoding.Struct, 0);

        throw new InvalidOperationException($"{member.DeclaringType?.Name}.{member.Name} has no wire encoding.");
    }
}
