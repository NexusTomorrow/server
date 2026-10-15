namespace NexusTomorrow.Network.Codec;

/// <summary>
/// The client's 16-bit packed float: a half-precision layout whose pack clamps out-of-range values to 0x43FF with the sign kept.
/// </summary>
internal static class PackedFloat
{
    internal static ushort Pack(float value)
    {
        uint bits = (uint)BitConverter.SingleToInt32Bits(value);
        uint magnitude = bits & 0x7FFFFFFFu;
        uint sign = (bits >> 16) & 0x8000u;

        if (magnitude < 0x33800000u)
            return (ushort)sign;
        if (magnitude <= 0x387FEFFFu)
            return (ushort)(sign | (((((bits & 0x7FFFFFu) | 0x800000u) >> (int)(113u - (magnitude >> 23))) + 4096u) >> 13));
        if (magnitude > 0x47FFEFFFu)
            return (ushort)(sign | 0x43FFu);
        return (ushort)(sign | ((magnitude - 0x37FFF000u) >> 13));
    }

    internal static float Unpack(ushort packed)
    {
        uint magnitude = packed & 0x7FFFu;
        uint sign = (packed & 0x8000u) << 16;

        if ((magnitude & 0x7C00u) != 0)
            return BitConverter.Int32BitsToSingle((int)(sign | ((magnitude + 0x1C000u) << 13)));
        if ((magnitude & 0x3FFu) == 0)
            return BitConverter.Int32BitsToSingle((int)(sign | magnitude));

        uint mantissa = (magnitude & 0x3FFu) << 13;
        uint exponent = 113u;
        while (mantissa <= 0x7FFFFFu)
        {
            mantissa *= 2;
            exponent--;
        }

        return BitConverter.Int32BitsToSingle((int)(sign | (exponent << 23) | (mantissa & 0x7FFFFFu)));
    }
}
