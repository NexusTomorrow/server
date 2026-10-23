namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A layout the codec reads and writes; the generator implements it for every [WireStruct] and [WireMessage] type.
/// </summary>
public interface IWireStruct
{
    /// <summary>
    /// Read this value's wire members from <paramref name="reader"/>, in declaration order.
    /// </summary>
    void Read(ref BitReader reader);

    /// <summary>
    /// Write this value's wire members to <paramref name="writer"/>, in declaration order.
    /// </summary>
    void Write(ref BitWriter writer);

    /// <summary>
    /// The number of bits <see cref="Write"/> emits for the current member values.
    /// </summary>
    int MeasureBits();

    /// <summary>
    /// The path from this value of the first wire member set to anything but its default whose meaning is not verified, or null when
    /// there is none. A member marked <see cref="VerifiedMemberAttribute"/> that holds a layout is searched in turn; an unmarked member
    /// must hold its whole value at the default. A marked union with an unmarked selector allows only its case-0 arm. Counts and
    /// selectors are judged through the array or union they describe, since <see cref="Write"/> derives them. Allocates nothing when
    /// it finds nothing.
    /// </summary>
    string? FindUnverifiedValue();

    /// <summary>
    /// True when every wire member is at its default: zero, false, an empty string or list, a fixed array of defaults, a layout at its
    /// defaults, or a union holding its case-0 arm at its defaults.
    /// </summary>
    bool IsDefault();
}
