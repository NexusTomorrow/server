using System.Numerics;
using System.Security.Cryptography;

namespace NexusTomorrow.Cryptography.Tests;

public class Srp6ProviderTests
{
    /// <summary>
    /// A deterministic 128-byte secret below N, the size the handshake produces.
    /// </summary>
    private static BigInteger Secret(int index)
    {
        byte[] bytes = new byte[128];
        SHA512.HashData(BitConverter.GetBytes(index)).CopyTo(bytes, 0);
        SHA512.HashData(BitConverter.GetBytes(~index)).CopyTo(bytes, 64);
        return new BigInteger(bytes, isUnsigned: true) % Srp6Provider.N;
    }

    [Fact]
    public void SessionKeyIsAlwaysSixtyFourBytes()
    {
        // 21 secrets in this range derive a key whose last byte is zero; index 4 is the first.
        for (int index = 1; index <= 4096; index++)
            Assert.Equal(64, Srp6Provider.DeriveSessionKey(Secret(index)).Length);
    }
}
