using System.Globalization;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Integer, bool and enum values as plain numbers, so values of different CLR types (two enums of one full name in two assemblies, a byte and a uint) compare and convert by value.
/// </summary>
internal static class ScalarValues
{
    public static bool IsInteger(Type type)
    {
        return type == typeof(bool) || type.IsEnum || Type.GetTypeCode(type) is TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16
            or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64;
    }

    /// <summary>
    /// The width of an integer type, or of an enum's underlying type, in bits.
    /// </summary>
    public static int SizeOf(Type type)
    {
        return Type.GetTypeCode(type) switch
        {
            TypeCode.Byte or TypeCode.SByte => 8,
            TypeCode.Int16 or TypeCode.UInt16 => 16,
            TypeCode.Int32 or TypeCode.UInt32 => 32,
            TypeCode.Int64 or TypeCode.UInt64 => 64,
            _ => throw new ArgumentException($"{type.Name} is not an integer or enum type.", nameof(type)),
        };
    }

    public static bool TryGetInteger(object value, out Int128 number)
    {
        object plain = value is Enum enumValue ? Convert.ChangeType(enumValue, enumValue.GetTypeCode(), CultureInfo.InvariantCulture) : value;
        Int128? result = plain switch
        {
            bool flag => flag ? Int128.One : Int128.Zero,
            byte integer => integer,
            sbyte integer => integer,
            short integer => integer,
            ushort integer => integer,
            int integer => integer,
            uint integer => integer,
            long integer => integer,
            ulong integer => integer,
            _ => null,
        };
        number = result.GetValueOrDefault();
        return result.HasValue;
    }

    /// <summary>
    /// <paramref name="number"/> as a <paramref name="target"/>, truncated to its width as an unchecked cast truncates.
    /// </summary>
    public static object FromInteger(Int128 number, Type target)
    {
        if (target == typeof(bool))
            return number != 0;
        if (target.IsEnum)
            return Enum.ToObject(target, FromInteger(number, Enum.GetUnderlyingType(target)));

        return Type.GetTypeCode(target) switch
        {
            TypeCode.Byte => (object)unchecked((byte)number),
            TypeCode.SByte => (object)unchecked((sbyte)number),
            TypeCode.Int16 => (object)unchecked((short)number),
            TypeCode.UInt16 => (object)unchecked((ushort)number),
            TypeCode.Int32 => (object)unchecked((int)number),
            TypeCode.UInt32 => (object)unchecked((uint)number),
            TypeCode.Int64 => (object)unchecked((long)number),
            TypeCode.UInt64 => (object)unchecked((ulong)number),
            _ => throw new ArgumentException($"{target.Name} is not an integer, bool or enum type.", nameof(target)),
        };
    }

    /// <summary>
    /// True when <paramref name="number"/>, read through the unsigned type of <paramref name="size"/> bits as the wire carries it, fits in <paramref name="bits"/> bits.
    /// </summary>
    public static bool FitsUnsigned(Int128 number, int bits, int size)
    {
        Int128 wire = number < 0 ? number + (Int128.One << size) : number;
        return wire >= 0 && wire < (Int128.One << bits);
    }

    public static bool FitsSigned(Int128 number, int bits)
    {
        Int128 limit = Int128.One << (bits - 1);
        return number >= -limit && number < limit;
    }
}
