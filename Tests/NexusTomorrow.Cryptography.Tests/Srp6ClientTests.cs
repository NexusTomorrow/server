using System.Security.Cryptography;

namespace NexusTomorrow.Cryptography.Tests;

public sealed class Srp6ClientTests
{
    [Fact]
    public void ClientAndServerAgreeOnTheSessionKeyAndBothEvidences()
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] verifier = Srp6Provider.GenerateVerifier(salt, "alice@example.com", "correct horse");
        var server = new Srp6Provider("alice@example.com", salt, verifier);
        byte[] b = server.GenerateServerCredentials();

        var client = new Srp6Client("alice@example.com", "correct horse");
        (byte[] a, byte[] m1) = client.Answer(salt, b);
        server.CalculateSecret(a);
        byte[] key = server.CalculateSessionKey();

        Assert.True(server.VerifyClientEvidenceMessage(m1));
        Assert.True(client.VerifyServerEvidence(server.CalculateServerEvidenceMessage()));
        Assert.Equal(key, client.SessionKey);
    }

    [Fact]
    public void AWrongPasswordFailsTheServersCheck()
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        var server = new Srp6Provider("alice@example.com", salt, Srp6Provider.GenerateVerifier(salt, "alice@example.com", "correct horse"));
        byte[] b = server.GenerateServerCredentials();
        (byte[] a, byte[] m1) = new Srp6Client("alice@example.com", "wrong").Answer(salt, b);
        server.CalculateSecret(a);
        server.CalculateSessionKey();
        Assert.False(server.VerifyClientEvidenceMessage(m1));
    }

    [Fact]
    public void AWrongServerEvidenceIsRefused()
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        var server = new Srp6Provider("alice@example.com", salt, Srp6Provider.GenerateVerifier(salt, "alice@example.com", "correct horse"));
        byte[] b = server.GenerateServerCredentials();
        var client = new Srp6Client("alice@example.com", "correct horse");
        (byte[] a, _) = client.Answer(salt, b);
        server.CalculateSecret(a);
        server.CalculateSessionKey();
        Assert.False(client.VerifyServerEvidence(new byte[32]));
    }
}
