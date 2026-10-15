using System.Globalization;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// How a [UnionCase] value meets its selector. A case value is held sign-extended to 64 bits; a [SInt] selector compares it as a signed number,
/// and a [UInt] selector compares the bits the generated Write puts on the wire: the value cast to the selector's type and read through the unsigned type of that size.
/// </summary>
internal static class UnionCases
{
    /// <summary>
    /// Whether the selector can carry the case: a [SInt] selector holds it as a signed number of its width; a [UInt] selector's type holds it, signed or unsigned, and its wire bits fit the width.
    /// </summary>
    public static bool Fits(ulong value, ScalarModel selector)
    {
        long signed = unchecked((long)value);
        if (selector.Encoding == ScalarEncoding.SInt)
            return selector.Bits == 64 || (signed >= -(1L << (selector.Bits - 1)) && signed < 1L << (selector.Bits - 1));

        bool typeHolds = selector.TypeBits == 64 || (signed >= -(1L << (selector.TypeBits - 1)) && signed < 1L << selector.TypeBits);
        return typeHolds && (selector.Bits == 64 || Label(value, selector) >> selector.Bits == 0);
    }

    /// <summary>
    /// The number the generated Read switches on for the case: the sign-extended value for a [SInt] selector, the wire bits for a [UInt] one.
    /// </summary>
    public static ulong Label(ulong value, ScalarModel selector)
    {
        if (selector.Encoding == ScalarEncoding.SInt || selector.TypeBits == 64)
            return value;
        return value & ((1UL << selector.TypeBits) - 1);
    }

    /// <summary>
    /// A label as C# source of the type the generated selector expression produces: long for a [SInt] selector, ulong for a [UInt] one.
    /// </summary>
    public static string Literal(ulong value, ScalarModel selector)
    {
        ulong label = Label(value, selector);
        return selector.Encoding == ScalarEncoding.SInt
            ? unchecked((long)label).ToString(CultureInfo.InvariantCulture) + "L"
            : label.ToString(CultureInfo.InvariantCulture) + "UL";
    }

    /// <summary>
    /// A label as a diagnostic shows it: signed for a [SInt] selector, unsigned for a [UInt] one.
    /// </summary>
    public static string LabelText(ulong value, ScalarModel selector)
    {
        ulong label = Label(value, selector);
        return selector.Encoding == ScalarEncoding.SInt
            ? unchecked((long)label).ToString(CultureInfo.InvariantCulture)
            : label.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// A case value as its declaration wrote it.
    /// </summary>
    public static string ValueText(ulong value)
    {
        long signed = unchecked((long)value);
        return signed < 0 ? signed.ToString(CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture);
    }
}
