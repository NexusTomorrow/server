namespace NexusTomorrow.Transport.Tests;

public sealed class PacketCipherTests
{
    private static readonly byte[] s_counting = Convert.FromHexString("000102030405060708090A0B0C0D0E0F");

    [Fact]
    public void BuildKeyIsTheClientsFixedKey()
    {
        Assert.Equal(0xD283F5B34A8DC685UL, PacketCipher.BuildKey);
    }

    [Fact]
    public void SessionKeyFoldsTheSixteenBytesOverTheBuildKey()
    {
        byte[] sessionKey = Convert.FromHexString("0102030405060708090A0B0C0D0E0F10");
        Assert.Equal(0xE096253F50C8934EUL, PacketCipher.SessionKey(sessionKey));
    }

    [Fact]
    public void SessionKeyRefusesAnythingButSixteenBytes()
    {
        Assert.Throws<ArgumentException>(() => PacketCipher.SessionKey(new byte[15]));
    }

    [Theory]
    [InlineData(0xD283F5B34A8DC685UL, "000102030405060708090A0B0C0D0E0F", "CF0D0C94CC5A042484E05EBEFE059315")]
    [InlineData(0xE096253F50C8934EUL, "000102030405060708090A0B0C0D0E0F", "263FB398DF9E62DAE325A1E68917FD38")]
    [InlineData(0xD283F5B34A8DC685UL, "9205AA3E00", "D36307900C")]
    public void EncryptMatchesTheKnownVectors(ulong key, string plain, string expected)
    {
        byte[] data = Convert.FromHexString(plain);
        new PacketCipher(key).Encrypt(data);
        Assert.Equal(expected, Convert.ToHexString(data));
    }

    [Theory]
    [InlineData(0xD283F5B34A8DC685UL, "CF0D0C94CC5A042484E05EBEFE059315", "000102030405060708090A0B0C0D0E0F")]
    [InlineData(0xD283F5B34A8DC685UL, "D36307900C", "9205AA3E00")]
    public void DecryptMatchesTheKnownVectors(ulong key, string cipher, string expected)
    {
        byte[] data = Convert.FromHexString(cipher);
        new PacketCipher(key).Decrypt(data);
        Assert.Equal(expected, Convert.ToHexString(data));
    }

    [Fact]
    public void DecryptInvertsEncryptAtEveryLengthUpToThreeBlocks()
    {
        var random = new Random(16042);
        var cipher = new PacketCipher(PacketCipher.BuildKey);
        for (int length = 0; length <= 24; length++)
        {
            byte[] plain = new byte[length];
            random.NextBytes(plain);
            byte[] data = [.. plain];
            cipher.Encrypt(data);
            cipher.Decrypt(data);
            Assert.Equal(plain, data);
        }
    }

    [Fact]
    public void OneInstanceEncryptsEachMessageFromTheSameState()
    {
        var cipher = new PacketCipher(PacketCipher.BuildKey);
        byte[] first = [.. s_counting];
        byte[] second = [.. s_counting];
        cipher.Encrypt(first);
        cipher.Encrypt(second);
        Assert.Equal(first, second);
    }

    [Fact]
    public void EncryptAndDecryptAllocateNothing()
    {
        var cipher = new PacketCipher(PacketCipher.BuildKey);
        byte[] data = new byte[256];
        cipher.Encrypt(data);
        cipher.Decrypt(data);

        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < 1000; i++)
        {
            cipher.Encrypt(data);
            cipher.Decrypt(data);
        }

        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
    }
}
