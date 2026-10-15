namespace NexusTomorrow.Cryptography;

public class Arc4Provider
{
    private uint _i;
    private uint _j;
    private readonly byte[] _s;

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

    public void Encrypt(byte[] buffer)
    {
        CycleBuffer(buffer);
    }

    public void Decrypt(byte[] buffer)
    {
        CycleBuffer(buffer);
    }

    private void CycleBuffer(byte[] buffer)
    {
        for (uint index = 0u; index < buffer.Length; index++)
        {
            _i = (_i + 1) & 0xFF;
            _j = (_j + _s[_i]) & 0xFF;
            Swap(_s, _i, _j);

            buffer[index] = (byte)((buffer[index]) ^ _s[(_s[_j] + _s[_i]) & 0xFF]);
        }
    }

    private static void Swap(byte[] s, uint i, uint j)
    {
        byte swap = s[i];
        s[i] = s[j];
        s[j] = swap;
    }
}
