using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using NexusTomorrow.Cryptography;

namespace NexusTomorrow.Gateway.Sts;

/// <summary>
/// Credentials for an email no account has (decisions.md D9 2b), so the exchange never says which accounts exist: a salt and a verifier
/// derived from the server's secret and the folded email, the same on every attempt. A verifier is any value below N, so without the
/// password no client can tell this one from a real one, and the proof always fails.
/// </summary>
public sealed class UnknownAccountCredentials
{
    private const int MinimumSecretBytes = 32;

    private readonly byte[] _secret;

    /// <summary>
    /// Create the source from <see cref="GatewayOptions.UnknownAccountSecret"/>.
    /// </summary>
    public UnknownAccountCredentials(IOptions<GatewayOptions> options)
    {
        _secret = Convert.FromBase64String(options.Value.UnknownAccountSecret);
        if (_secret.Length < MinimumSecretBytes)
            throw new InvalidOperationException($"{GatewayOptions.Section}:{nameof(GatewayOptions.UnknownAccountSecret)} must be base64 of at least {MinimumSecretBytes} bytes.");
    }

    /// <summary>
    /// The salt and verifier for <paramref name="loginName"/>.
    /// </summary>
    public (byte[] Salt, byte[] Verifier) For(string loginName)
    {
        string folded = loginName.ToLowerInvariant();
        byte[] salt = HMACSHA256.HashData(_secret, Encoding.UTF8.GetBytes($"salt\n{folded}"))[..16];
        Span<byte> wide = stackalloc byte[128];
        for (int part = 0; part < 4; part++)
            HMACSHA256.HashData(_secret, Encoding.UTF8.GetBytes($"verifier\n{part}\n{folded}"), wide.Slice(part * 32, 32));
        BigInteger verifier = new BigInteger(wide, isUnsigned: true) % Srp6Provider.N;
        return (salt, (verifier.IsZero ? BigInteger.One : verifier).ToByteArray(isUnsigned: true));
    }
}
