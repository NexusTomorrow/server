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
}
