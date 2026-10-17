using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace NexusTomorrow.Cryptography;

/// <summary>
/// The client's half of the STS's SRP6 exchange, for headless clients and tests. The identity is the lowercased email, the form the
/// verifier was made with (<see cref="PasswordProvider"/>).
/// </summary>
public sealed class Srp6Client
{
    private readonly string _identity;
    private readonly string _password;
    private BigInteger _clientPublic;
    private BigInteger _clientEvidence;
    private BigInteger _sessionKey;

    /// <summary>
    /// Create the client half for <paramref name="identity"/> and <paramref name="password"/>.
    /// </summary>
    public Srp6Client(string identity, string password)
    {
        _identity = identity;
        _password = password;
    }

    /// <summary>
    /// The session key K, once <see cref="Answer"/> has run.
    /// </summary>
    public byte[] SessionKey { get; private set; } = [];

    /// <summary>
    /// Answer the server's salt and public ephemeral B with the client's public ephemeral A and evidence M1.
    /// </summary>
    public (byte[] A, byte[] M1) Answer(byte[] salt, byte[] serverB)
    {
        BigInteger n = Srp6Provider.N;
        BigInteger g = Srp6Provider.g;
        var s = new BigInteger(salt, isUnsigned: true);
        var b = new BigInteger(serverB, isUnsigned: true);

        BigInteger a = RandomProvider.GetUnsignedBigInteger(128u);
        _clientPublic = BigInteger.ModPow(g, a, n);
        BigInteger k = Srp6Provider.Hash(true, n, g);
        BigInteger u = Srp6Provider.Hash(true, _clientPublic, b);
        byte[] p = SHA256.HashData(Encoding.UTF8.GetBytes($"{_identity}:{_password}"));
        BigInteger x = Srp6Provider.Hash(true, s, new BigInteger(p, isUnsigned: true));
        var v = BigInteger.ModPow(g, x, n);
        BigInteger @base = (((b - (k * v)) % n) + n) % n;
        var shared = BigInteger.ModPow(@base, a + (u * x), n);

        SessionKey = Srp6Provider.DeriveSessionKey(shared);
        _sessionKey = new BigInteger(SessionKey, isUnsigned: true);
        byte[] identityHash = SHA256.HashData(Encoding.UTF8.GetBytes(_identity));
        _clientEvidence = Srp6Provider.Hash(false, Srp6Provider.Hash(false, n) ^ Srp6Provider.Hash(false, g),
            new BigInteger(identityHash, isUnsigned: true), s, _clientPublic, b, _sessionKey);
        return (_clientPublic.ToByteArray(isUnsigned: true), _clientEvidence.ToByteArray(isUnsigned: true));
    }

    /// <summary>
    /// True when <paramref name="m2"/> is the evidence a server holding the verifier would send.
    /// </summary>
    public bool VerifyServerEvidence(byte[] m2)
    {
        byte[] expected = Srp6Provider.Hash(true, _clientPublic, _clientEvidence, _sessionKey).ToByteArray(isUnsigned: true);
        Srp6Provider.ReverseBytesAsUInt32(expected);
        return CryptographicOperations.FixedTimeEquals(expected, m2);
    }
}
