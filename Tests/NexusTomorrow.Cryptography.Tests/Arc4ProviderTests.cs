using System.Text;

namespace NexusTomorrow.Cryptography.Tests;

public sealed class Arc4ProviderTests
{
    [Fact]
    public void ApplyMatchesThePublishedVector()
    {
        byte[] data = Encoding.ASCII.GetBytes("Plaintext");
        new Arc4Provider(Encoding.ASCII.GetBytes("Key")).Apply(data);
        Assert.Equal("BBF316E8D940AF0AD3", Convert.ToHexString(data));
    }

    [Fact]
    public void ApplyingInPiecesContinuesTheStream()
    {
        byte[] whole = Encoding.ASCII.GetBytes("Plaintext");
        new Arc4Provider(Encoding.ASCII.GetBytes("Key")).Apply(whole);
        byte[] pieces = Encoding.ASCII.GetBytes("Plaintext");
        var cipher = new Arc4Provider(Encoding.ASCII.GetBytes("Key"));
        cipher.Apply(pieces.AsSpan(0, 4));
        cipher.Apply(pieces.AsSpan(4));
        Assert.Equal(whole, pieces);
    }

    [Fact]
    public void EncryptAndDecryptContinueTheSameStreamAsApply()
    {
        byte[] viaApply = Encoding.ASCII.GetBytes("Plaintext");
        new Arc4Provider(Encoding.ASCII.GetBytes("Key")).Apply(viaApply);
        byte[] viaEncrypt = Encoding.ASCII.GetBytes("Plaintext");
        new Arc4Provider(Encoding.ASCII.GetBytes("Key")).Encrypt(viaEncrypt);
        Assert.Equal(viaApply, viaEncrypt);
    }
}
