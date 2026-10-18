namespace NexusTomorrow.Cryptography;

/// <summary>
/// The RC4 stream cipher the STS applies, keyed with the SRP6 session key. One instance is one direction's stream: it carries its state
/// from call to call, so every byte of the direction goes through the same instance, in order.
/// </summary>
public class Arc4Provider
{
    private uint _i;
    private uint _j;
    private readonly byte[] _s;

    /// <summary>
    /// Create a stream keyed with <paramref name="key"/>.
    /// </summary>
    public Arc4Provider(byte[] key)
    {
        _i = 0;
        _j = 0;
        _s = [.. Enumerable.Range(0, 256).Select(i => (byte)i)];

        for (uint index = 0u, index2 = 0u; index < 256u; index++)
        {
            index2 = (index2 + key[index % key.Length] + _s[index]) & 0xFF;
            Swap(_s, index, index2);
        }
    }

    /// <summary>
    /// XOR <paramref name="data"/> with the next bytes of the stream, in place. Encrypting and decrypting are the same operation.
    /// </summary>
    public void Apply(Span<byte> data)
    {
        for (int index = 0; index < data.Length; index++)
        {
            _i = (_i + 1) & 0xFF;
            _j = (_j + _s[_i]) & 0xFF;
            Swap(_s, _i, _j);

            data[index] = (byte)(data[index] ^ _s[(_s[_j] + _s[_i]) & 0xFF]);
        }
    }

    /// <summary>
    /// Encrypt <paramref name="buffer"/> in place.
    /// </summary>
    public void Encrypt(byte[] buffer)
    {
        Apply(buffer);
    }

    /// <summary>
    /// Decrypt <paramref name="buffer"/> in place.
    /// </summary>
    public void Decrypt(byte[] buffer)
    {
        Apply(buffer);
    }

    private static void Swap(byte[] s, uint i, uint j)
    {
        byte swap = s[i];
        s[i] = s[j];
        s[j] = swap;
    }
}
