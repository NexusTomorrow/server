using System.Buffers;
using System.Buffers.Binary;
using System.Globalization;
using System.IO.Pipelines;
using System.Net;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Cryptography;
using NexusTomorrow.Gateway.Limits;
using NexusTomorrow.Network.Sts;
using NexusTomorrow.Network.Sts.Model;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;

namespace NexusTomorrow.Gateway.Sts;

/// <summary>
/// One STS connection. Each URI is accepted only in its state; anything out of order ends the connection without an answer. Bytes from
/// the client are RC4-decrypted from the end of the <c>KeyData</c> request on, including bytes already buffered behind it; bytes to the
/// client from the end of the <c>KeyData</c> response on.
/// </summary>
public sealed class StsConnection
{
    // The longest email address, so a longer name cannot be an account.
    private const int MaxLoginNameChars = 254;

    // N is 1024 bits, so A and B are at most 128 bytes, and M1 is a SHA-256 hash.
    private const int MaxPublicValueBytes = 128;
    private const int MaxEvidenceBytes = 32;

    private static readonly XmlReaderSettings s_xml = new()
    {
        DtdProcessing = DtdProcessing.Prohibit,
        XmlResolver = null,
        MaxCharactersInDocument = StsRequestReader.MaxBodyBytes,
    };

    private readonly IAccountDirectory _directory;
    private readonly IWriteBehind _persistence;
    private readonly AccountOwner _accounts;
    private readonly UnknownAccountCredentials _unknown;
    private readonly LoginRateLimiter _limiter;
    private readonly GatewayOptions _options;
    private readonly TimeProvider _time;
    private readonly ILogger<StsConnection> _logger;

    private State _state = State.New;
    private string _endReason = "request refused";
    private AccountCredentials? _account;
    private string _loginName = "";
    private Srp6Provider? _exchange;
    private Arc4Provider? _receive;
    private Arc4Provider? _send;
    private byte[] _plain = [];

    /// <summary>
    /// Create the connection.
    /// </summary>
    public StsConnection(
        IAccountDirectory directory,
        IWriteBehind persistence,
        AccountOwner accounts,
        UnknownAccountCredentials unknown,
        LoginRateLimiter limiter,
        IOptions<GatewayOptions> options,
        TimeProvider time,
        ILogger<StsConnection> logger)
    {
        _directory = directory;
        _persistence = persistence;
        _accounts = accounts;
        _unknown = unknown;
        _limiter = limiter;
        _options = options.Value;
        _time = time;
        _logger = logger;
    }

    private enum State
    {
        New,
        Connected,
        LoginStarted,
        Authenticated,
    }

    /// <summary>
    /// Serve the connection until the client closes it, it misbehaves, or <paramref name="cancellationToken"/> fires.
    /// </summary>
    public async Task RunAsync(IDuplexPipe transport, IPEndPoint remote, CancellationToken cancellationToken)
    {
        // The pre-auth timer runs on the injected clock; the game token's issue lifts it.
        using var preAuth = new CancellationTokenSource(_options.PreAuthTimeout, _time);
        using var lifetime = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, preAuth.Token);
        string reason = "the client closed the connection";
        try
        {
            while (true)
            {
                ReadResult result = await transport.Input.ReadAsync(lifetime.Token);
                byte[] chunk = result.Buffer.ToArray();
                transport.Input.AdvanceTo(result.Buffer.End);
                _receive?.Apply(chunk);
                _plain = [.. _plain, .. chunk];

                while (true)
                {
                    var buffer = new ReadOnlySequence<byte>(_plain);
                    StsReadStatus status = StsRequestReader.TryRead(ref buffer, out StsRequest? request);
                    if (status == StsReadStatus.Incomplete)
                        break;
                    if (status == StsReadStatus.Malformed)
                    {
                        reason = "malformed request";
                        return;
                    }

                    _plain = buffer.ToArray();
                    bool keyWasOff = _receive is null;
                    if (!await HandleAsync(request!, transport.Output, remote, preAuth, lifetime.Token))
                    {
                        reason = _endReason;
                        return;
                    }

                    // The client turns its key on right after sending KeyData, so whatever followed it in this read is ciphertext.
                    if (keyWasOff && _receive is not null)
                        _receive.Apply(_plain);
                }

                if (result.IsCompleted)
                    return;
            }
        }
        catch (OperationCanceledException)
        {
            reason = preAuth.IsCancellationRequested ? "the login did not finish in time" : "the server is stopping";
        }
        catch (Exception exception) when (IsMalformedInput(exception))
        {
            // The exception's text can quote the client's bytes, so only the fact is logged.
            reason = "malformed request";
        }
        catch (Exception exception)
        {
            reason = "unexpected failure";
            GatewayLog.StsFailed(_logger, exception);
        }
        finally
        {
            GatewayLog.StsConnectionEnded(_logger, remote, reason);
            await transport.Output.CompleteAsync();
            await transport.Input.CompleteAsync();
        }
    }

    private static bool IsMalformedInput(Exception exception) =>
        exception is XmlException or InvalidDataException or FormatException or OverflowException or EndOfStreamException
            or ArgumentException or InvalidCastException;

    private static T Read<T>(string body)
        where T : IReadable, new()
    {
        var document = new XmlDocument();
        using (var reader = XmlReader.Create(new StringReader(body), s_xml))
            document.Load(reader);

        var message = new T();
        message.Read(document);
        return message;
    }

    // Length-prefixed parts, the shape of the KeyData values: each part's int32 length, then its bytes.
    private static string Pack(params ReadOnlySpan<byte[]> parts)
    {
        int size = 0;
        foreach (byte[] part in parts)
            size += sizeof(int) + part.Length;

        byte[] packed = new byte[size];
        int offset = 0;
        foreach (byte[] part in parts)
        {
            BinaryPrimitives.WriteInt32LittleEndian(packed.AsSpan(offset), part.Length);
            part.CopyTo(packed, offset + sizeof(int));
            offset += sizeof(int) + part.Length;
        }

        return Convert.ToBase64String(packed);
    }

    // The request's sequence number goes back in the response header, so it is echoed only when it is plain digits.
    private static string SequenceOf(StsRequest request) =>
        request.Headers.TryGetValue("s", out string? sequence) && sequence.Length is > 0 and <= 10 && sequence.All(char.IsAsciiDigit)
            ? sequence
            : "0";

    // Names the URIs the server knows and nothing else, so no client text reaches a log line.
    private static string KnownUri(string uri) =>
        uri is "/Sts/Ping" or "/Sts/Connect" or "/Auth/LoginStart" or "/Auth/KeyData" or "/Auth/LoginFinish"
            or "/GameAccount/ListMyAccounts" or "/Auth/RequestGameToken"
            ? uri
            : "(unknown)";

    private async Task<bool> HandleAsync(StsRequest request, PipeWriter output, IPEndPoint remote, CancellationTokenSource preAuth, CancellationToken cancellationToken)
    {
        switch (request.Uri)
        {
            case "/Sts/Ping":
                return true;
            case "/Sts/Connect" when _state == State.New:
                Read<ClientConnectMessage>(request.Body);
                _state = State.Connected;
                return true;
            case "/Auth/LoginStart" when _state == State.Connected:
                return await LoginStartAsync(request, output, remote, cancellationToken);
            case "/Auth/KeyData" when _state == State.LoginStarted:
                return await KeyDataAsync(request, output, cancellationToken);
            case "/Auth/LoginFinish" when _state == State.Authenticated && _account is { } account:
                await ReplyAsync(request, output, 200, new ServerLoginFinishMessage
                {
                    LocationId = "",
                    UserId = UserIdOf(account),
                    UserCenter = 0,
                    UserName = _loginName,
                    AccessMask = 1,
                }, cancellationToken);
                return true;
            case "/GameAccount/ListMyAccounts" when _state == State.Authenticated && _account is { } account:
                await ReplyAsync(request, output, 200, new ListMyAccountsResponse
                {
                    Alias = _loginName,
                    Created = "",
                    GameAccountId = account.Id.ToString(CultureInfo.InvariantCulture),
                }, cancellationToken);
                return true;
            case "/Auth/RequestGameToken" when _state == State.Authenticated && _account is { } account:
                return await RequestGameTokenAsync(request, output, account, preAuth, cancellationToken);
            default:
                GatewayLog.StsRequestRefused(_logger, KnownUri(request.Uri), _state.ToString());
                _endReason = "request out of order";
                return false;
        }
    }

    private async Task<bool> LoginStartAsync(StsRequest request, PipeWriter output, IPEndPoint remote, CancellationToken cancellationToken)
    {
        if (!_limiter.TryAttempt(remote.Address))
        {
            _endReason = "too many login attempts from the address";
            await ReplyAsync(request, output, 400, new ServerErrorMessage(StsErrors.InvalidAccountNameOrPassword), cancellationToken);
            return false;
        }

        if (!_persistence.IsAvailable)
        {
            _endReason = "the database is unavailable";
            await ReplyAsync(request, output, 400, new ServerErrorMessage(StsErrors.InvalidAccountNameOrPassword), cancellationToken);
            return false;
        }

        string loginName = Read<ClientLoginStartMessage>(request.Body).LoginName;
        AccountCredentials? account = null;
        if (loginName.Length <= MaxLoginNameChars)
        {
            try
            {
                account = await _directory.FindAsync(loginName, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                GatewayLog.StsFailed(_logger, exception);
                _endReason = "the account lookup failed";
                await ReplyAsync(request, output, 400, new ServerErrorMessage(StsErrors.InvalidAccountNameOrPassword), cancellationToken);
                return false;
            }
        }

        // An unknown account runs the same exchange on credentials derived from the server's secret, so it fails at the proof.
        (byte[] salt, byte[] verifier) = account is null ? _unknown.For(loginName) : (account.Salt, account.Verifier);
        var exchange = new Srp6Provider(loginName, salt, verifier);
        byte[] serverPublic = exchange.GenerateServerCredentials();
        _exchange = exchange;
        _account = account;
        _loginName = loginName;
        _state = State.LoginStarted;
        await ReplyAsync(request, output, 200, new ServerLoginStartMessage { KeyData = Pack(salt, serverPublic) }, cancellationToken);
        return true;
    }

    private async Task<bool> KeyDataAsync(StsRequest request, PipeWriter output, CancellationToken cancellationToken)
    {
        ClientKeyDataMessage message = Read<ClientKeyDataMessage>(request.Body);
        if (_exchange is not { } exchange)
        {
            _endReason = "request out of order";
            return false;
        }

        byte[] sessionKey = [];
        bool proven = false;
        bool usable = message.A.Length is > 0 and <= MaxPublicValueBytes && message.M1.Length <= MaxEvidenceBytes;
        if (usable)
        {
            try
            {
                exchange.CalculateSecret(message.A);
                sessionKey = exchange.CalculateSessionKey();
                proven = exchange.VerifyClientEvidenceMessage(message.M1);
            }
            catch (CryptographicException)
            {
                usable = false;
            }
        }

        if (!usable)
        {
            _endReason = "unusable key exchange values";
            await ReplyAsync(request, output, 400, new ServerErrorMessage(StsErrors.InvalidAccountNameOrPassword), cancellationToken);
            return false;
        }

        AccountCredentials? account = _account;
        if (!proven || account is null)
        {
            _exchange = null;
            _account = null;
            _state = State.Connected;
            await ReplyAsync(request, output, 400, new ServerErrorMessage(StsErrors.InvalidAccountNameOrPassword), cancellationToken);
            return true;
        }

        // The answer to the proof goes in clear; the key turns on for everything after it.
        await ReplyAsync(request, output, 200, new ServerKeyDataMessage { KeyData = Pack(exchange.CalculateServerEvidenceMessage()) }, cancellationToken);
        _receive = new Arc4Provider(sessionKey);
        _send = new Arc4Provider(sessionKey);
        _exchange = null;
        _state = State.Authenticated;
        return true;
    }

    private async Task<bool> RequestGameTokenAsync(StsRequest request, PipeWriter output, AccountCredentials account, CancellationTokenSource preAuth, CancellationToken cancellationToken)
    {
        var reply = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (!_accounts.Post(new IssueGameToken(account, reply)))
        {
            _endReason = "the account owner is stopped";
            await ReplyAsync(request, output, 400, new ServerErrorMessage(StsErrors.InvalidAccountNameOrPassword), cancellationToken);
            return false;
        }

        Guid token = await reply.Task.WaitAsync(cancellationToken);
        await ReplyAsync(request, output, 200, new RequestGameTokenResponse { Token = token.ToString() }, cancellationToken);
        preAuth.CancelAfter(Timeout.InfiniteTimeSpan);
        return true;
    }

    // The client parses this into LoginRequest field 3, which the server does not read: the account id in the first eight bytes,
    // little-endian, and zeros after.
    private static string UserIdOf(AccountCredentials account)
    {
        Span<byte> bytes = stackalloc byte[16];
        BinaryPrimitives.WriteInt64LittleEndian(bytes, account.Id);
        return new Guid(bytes).ToString();
    }

    private async Task ReplyAsync(StsRequest request, PipeWriter output, int status, IWritable message, CancellationToken cancellationToken)
    {
        byte[] bytes = StsResponseWriter.Write(status, status == 200 ? "OK" : "Bad Request", SequenceOf(request), StsResponseWriter.Xml(message) + "\n");
        _send?.Apply(bytes);
        await output.WriteAsync(bytes, cancellationToken);
    }
}
