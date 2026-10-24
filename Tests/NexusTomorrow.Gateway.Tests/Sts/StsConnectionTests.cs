using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Cryptography;
using NexusTomorrow.Gateway.Limits;
using NexusTomorrow.Gateway.Sts;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Gateway.Tests.Sts;

public sealed class StsConnectionTests : IAsyncLifetime
{
    private const string Connect = "<Connect>\n<ConnType>400</ConnType>\n</Connect>\n";
    private static readonly TimeSpan s_wait = TimeSpan.FromSeconds(10);

    private readonly FakeTimeProvider _time = new();
    private readonly FakeWriteBehind _persistence = new();
    private readonly Directory _directory = new();
    private readonly AccountOwner _accounts;
    private readonly IOptions<GatewayOptions> _options = Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret() });
    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private readonly List<Task> _runs = [];

    public StsConnectionTests()
    {
        _accounts = new AccountOwner(_time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, new FakeWriteBehind(), FixedEntitlementCaps.Defaults());
        _directory.Add("alice@example.com", "correct horse");
    }

    public ValueTask InitializeAsync()
    {
        _accounts.Start();
        Serve(_pipes, 50000);
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        await Task.WhenAll(_runs).WaitAsync(s_wait);
        await _accounts.StopAsync().WaitAsync(s_wait);
    }

    [Fact]
    public async Task TheRightPasswordGetsAGameTokenTheAccountOwnerRedeems()
    {
        var client = new StsTestClient(_pipes.Client);
        (Guid? token, StsReply? error) = await client.LoginAsync("alice@example.com", "correct horse");
        Assert.Null(error);
        var reply = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new RedeemGameToken(token!.Value, "alice@example.com", reply));
        Assert.IsType<LoginGranted>(await reply.Task.WaitAsync(s_wait, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AWrongPasswordGetsError3002()
    {
        (Guid? token, StsReply? error) = await new StsTestClient(_pipes.Client).LoginAsync("alice@example.com", "wrong");
        Assert.Null(token);
        Assert.Equal(400, error!.Status);
        Assert.Equal("3002", error.Xml().DocumentElement!.GetAttribute("code"));
    }

    [Fact]
    public async Task AnUnknownAccountGetsAStableSaltAndTheSameErrorAsAWrongPassword()
    {
        var client = new StsTestClient(_pipes.Client);
        byte[] salt1 = await LoginStartSaltAsync(client, "nobody@example.com");
        (Guid? token, StsReply? error) = await new StsTestClient(OpenSecond()).LoginAsync("nobody@example.com", "anything");
        Assert.Null(token);
        Assert.Equal("3002", error!.Xml().DocumentElement!.GetAttribute("code"));

        byte[] salt2 = new UnknownAccountCredentials(_options).For("NOBODY@example.com").Salt;
        Assert.Equal(salt1, salt2);
        Assert.Equal(16, salt1.Length);
    }

    [Fact]
    public async Task AnUnknownAccountAnswersWithTheSameShapeAsAKnownOne()
    {
        (byte[] knownSalt, byte[] knownB) = await LoginStartAsync(new StsTestClient(_pipes.Client), "alice@example.com");
        (byte[] unknownSalt, byte[] unknownB) = await LoginStartAsync(new StsTestClient(OpenSecond()), "nobody@example.com");
        Assert.Equal(knownSalt.Length, unknownSalt.Length);
        Assert.InRange(knownB.Length, 120, 128);
        Assert.InRange(unknownB.Length, 120, 128);
    }

    [Fact]
    public async Task AnUnknownAccountCostsTheSameDirectoryReadAsAKnownOne()
    {
        await new StsTestClient(_pipes.Client).LoginAsync("alice@example.com", "wrong");
        Assert.Equal(1, _directory.Reads);
        await new StsTestClient(OpenSecond()).LoginAsync("nobody@example.com", "wrong");
        Assert.Equal(2, _directory.Reads);
    }

    [Fact]
    public async Task RequestGameTokenBeforeTheProofEndsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Connect", Connect);
        await client.SendAsync("/Auth/LoginStart", "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n");
        await client.ReceiveAsync();
        await client.SendAsync("/Auth/RequestGameToken", "");
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task RequestGameTokenAfterAFailedProofEndsTheConnectionWithoutAToken()
    {
        var client = new StsTestClient(_pipes.Client);
        await ProveAsync(client, "alice@example.com", "wrong");
        Assert.Equal(400, (await client.ReceiveAsync()).Status);
        await client.SendAsync("/Auth/RequestGameToken", "");
        Assert.True(await client.IsClosedAsync(s_wait));
        Assert.Equal(0L, _accounts.Handled);
    }

    [Theory]
    [InlineData("/Auth/LoginStart")]
    [InlineData("/Auth/KeyData")]
    [InlineData("/Auth/LoginFinish")]
    [InlineData("/GameAccount/ListMyAccounts")]
    [InlineData("/Auth/RequestGameToken")]
    [InlineData("/Auth/PageVerifiedIps")]
    [InlineData("/Sts/Nonsense")]
    public async Task AnyUriOutOfItsStateOrUnknownEndsTheConnectionWithoutAnAnswer(string uri)
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync(uri, "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n");
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task ASecondConnectEndsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Connect", Connect);
        await client.SendAsync("/Sts/Connect", Connect);
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task APingAtAnyTimeGetsNoAnswer()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Ping", "<Ping>\n</Ping>\n");
        await client.SendAsync("/Sts/Connect", Connect);
        await client.SendAsync("/Sts/Ping", "<Ping>\n</Ping>\n");
        await client.SendAsync("/Auth/LoginStart", "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n");
        StsReply first = await client.ReceiveAsync();
        Assert.Single(first.Xml().GetElementsByTagName("KeyData"));
    }

    [Fact]
    public async Task AFailedProofLeavesTheConnectionReadyForAnotherLoginStart()
    {
        var client = new StsTestClient(_pipes.Client);
        await ProveAsync(client, "alice@example.com", "wrong");
        Assert.Equal(400, (await client.ReceiveAsync()).Status);
        (byte[] salt, _) = await LoginStartAsync(client, "alice@example.com", connect: false);
        Assert.Equal(16, salt.Length);
    }

    [Fact]
    public async Task ARequestInTheSameWriteAsKeyDataIsDecrypted()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Connect", Connect);
        await client.SendAsync("/Auth/LoginStart", "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n");
        (byte[] salt, byte[] b) = StsTestClient.ReadTwo((await client.ReceiveAsync()).Xml().GetElementsByTagName("KeyData")[0]!.InnerText);
        var srp = new Srp6Client("alice@example.com", "correct horse");
        (byte[] a, byte[] m1) = srp.Answer(salt, b);

        // KeyData in clear, then LoginFinish under the new key, in one write.
        await client.SendTogetherAsync(
            ("/Auth/KeyData", $"<Request>\n<KeyData>{StsTestClient.WriteTwo(a, m1)}</KeyData>\n</Request>\n", true, srp.SessionKey),
            ("/Auth/LoginFinish", "", false, null));
        Assert.Equal(200, (await client.ReceiveAsync()).Status);
        client.ReceiveKeyOn(srp.SessionKey);
        StsReply finish = await client.ReceiveAsync();
        Assert.Equal(200, finish.Status);
        Assert.Contains("<UserName>", finish.Body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LoginFinishAnswersWithTheAccountsNameAndAUserIdFromItsId()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.LoginAsync("alice@example.com", "correct horse");
        await client.SendAsync("/Auth/LoginFinish", "");
        StsReply finish = await client.ReceiveAsync();
        Assert.Equal("alice@example.com", finish.Xml().GetElementsByTagName("UserName")[0]!.InnerText);
        Assert.Equal("00000001-0000-0000-0000-000000000000", finish.Xml().GetElementsByTagName("UserId")[0]!.InnerText);
    }

    [Fact]
    public async Task ListMyAccountsAnswersWithTheAccountsNameAndId()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.LoginAsync("alice@example.com", "correct horse");
        await client.SendAsync("/GameAccount/ListMyAccounts", "");
        StsReply list = await client.ReceiveAsync();
        Assert.Equal("alice@example.com", list.Xml().GetElementsByTagName("Alias")[0]!.InnerText);
        Assert.Equal("1", list.Xml().GetElementsByTagName("GameAccountId")[0]!.InnerText);
    }

    [Fact]
    public async Task PageVerifiedIpsAnswersWithAnEmptyItemsListAndKeepsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.LoginAsync("alice@example.com", "correct horse");
        await client.SendAsync("/Auth/PageVerifiedIps", "<Request>\n<PageIndex>1</PageIndex>\n</Request>\n");
        StsReply page = await client.ReceiveAsync();
        Assert.Equal(200, page.Status);
        XmlElement items = Assert.IsType<XmlElement>(Assert.Single(page.Xml().GetElementsByTagName("Items")));
        Assert.False(items.HasChildNodes);

        await client.SendAsync("/GameAccount/ListMyAccounts", "");
        Assert.Equal(200, (await client.ReceiveAsync()).Status);
    }

    [Fact]
    public async Task ABodyDeliveredInPiecesIsReadWhole()
    {
        var client = new StsTestClient(_pipes.Client);
        byte[] connect = Encoding.UTF8.GetBytes($"POST /Sts/Connect STS/1.0\r\nl:{Connect.Length}\r\ns:1\r\n\r\n{Connect}");
        await client.SendRawAsync(connect[..40]);
        await client.SendRawAsync(connect[40..60]);
        await client.SendRawAsync(connect[60..]);
        (byte[] salt, _) = await LoginStartAsync(client, "alice@example.com", connect: false);
        Assert.Equal(16, salt.Length);
    }

    [Fact]
    public async Task ALoginNameWithMultiByteCharactersCountsItsLengthInBytes()
    {
        (Guid? token, StsReply? error) = await new StsTestClient(_pipes.Client).LoginAsync("élève@example.com", "anything");
        Assert.Null(token);
        Assert.Equal("3002", error!.Xml().DocumentElement!.GetAttribute("code"));
    }

    [Fact]
    public async Task AHeadPastTheCapEndsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendRawAsync(new byte[StsRequestReader.MaxHeaderBytes + 1]);
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task ABodyLengthPastTheCapEndsTheConnectionBeforeTheBodyArrives()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendRawAsync(Encoding.ASCII.GetBytes($"POST /Sts/Connect STS/1.0\r\nl:{StsRequestReader.MaxBodyBytes + 1}\r\n\r\n"));
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task ABodyThatIsNotXmlEndsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Connect", "not xml at all");
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task AKeyDataPartLongerThanItsDataEndsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await LoginStartAsync(client, "alice@example.com");
        string keyData = Convert.ToBase64String([0xFF, 0xFF, 0xFF, 0x7F, 1]);
        await client.SendAsync("/Auth/KeyData", $"<Request>\n<KeyData>{keyData}</KeyData>\n</Request>\n");
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task AClientPublicValueOfZeroIsRefusedAndEndsTheConnection()
    {
        var client = new StsTestClient(_pipes.Client);
        await LoginStartAsync(client, "alice@example.com");
        await client.SendAsync("/Auth/KeyData", $"<Request>\n<KeyData>{StsTestClient.WriteTwo([0], new byte[32])}</KeyData>\n</Request>\n");
        Assert.Equal(400, (await client.ReceiveAsync()).Status);
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task LoginsAreRefusedWhileTheDatabaseIsUnavailable()
    {
        _persistence.IsAvailable = false;
        (Guid? token, StsReply? error) = await new StsTestClient(_pipes.Client).LoginAsync("alice@example.com", "correct horse");
        Assert.Null(token);
        Assert.Equal(400, error!.Status);
    }

    [Fact]
    public async Task AFailingDirectoryEndsTheConnectionAndHandsOutNoCredentials()
    {
        _directory.Fail = true;
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Connect", Connect);
        await client.SendAsync("/Auth/LoginStart", "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n");
        Assert.Equal(400, (await client.ReceiveAsync()).Status);
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task AnAddressOverItsLoginAttemptsIsRefusedAndEnded()
    {
        var limiter = new LoginRateLimiter(_time, Options.Create(new GatewayOptions { UnknownAccountSecret = LoginRateLimiterTests.Secret(), LoginAttemptsPerMinute = 1 }));
        var first = new PipePair();
        var second = new PipePair();
        Serve(first, 50010, limiter);
        Serve(second, 50011, limiter);

        var firstClient = new StsTestClient(first.Client);
        await LoginStartAsync(firstClient, "alice@example.com");

        var secondClient = new StsTestClient(second.Client);
        await secondClient.SendAsync("/Sts/Connect", Connect);
        await secondClient.SendAsync("/Auth/LoginStart", "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n");
        Assert.Equal(400, (await secondClient.ReceiveAsync()).Status);
        Assert.True(await secondClient.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task AConnectionThatDoesNotFinishItsLoginInTimeIsClosed()
    {
        var client = new StsTestClient(_pipes.Client);
        await client.SendAsync("/Sts/Connect", Connect);
        _time.Advance(_options.Value.PreAuthTimeout + TimeSpan.FromSeconds(1));
        Assert.True(await client.IsClosedAsync(s_wait));
    }

    // The clock passes the limit inside the write of the token's reply, before the server's next line runs, so the limit must already
    // be lifted when that reply is written.
    [Fact]
    public async Task TheTimeLimitIsLiftedOnceTheGameTokenIsIssued()
    {
        var pipes = new PipePair();
        var output = new ClockStepWriter(pipes.Server.Output, _time);
        Serve(new Transport(pipes.Server.Input, output), 50040);
        var client = new StsTestClient(pipes.Client);
        (byte[] salt, byte[] b) = await LoginStartAsync(client, "alice@example.com");
        var srp = new Srp6Client("alice@example.com", "correct horse");
        (byte[] a, byte[] m1) = srp.Answer(salt, b);
        await client.SendAsync("/Auth/KeyData", $"<Request>\n<KeyData>{StsTestClient.WriteTwo(a, m1)}</KeyData>\n</Request>\n");
        Assert.Equal(200, (await client.ReceiveAsync()).Status);
        client.KeyOn(srp.SessionKey);

        output.StepOnNextFlush(_options.Value.PreAuthTimeout + TimeSpan.FromSeconds(1));
        await client.SendAsync("/Auth/RequestGameToken", "");
        Assert.Equal(200, (await client.ReceiveAsync()).Status);
        await client.SendAsync("/GameAccount/ListMyAccounts", "");
        Assert.Equal(200, (await client.ReceiveAsync()).Status);
    }

    [Fact]
    public async Task AMixedCaseLoginNameProvesAgainstTheVerifierOfTheFoldedName()
    {
        (Guid? token, StsReply? error) = await new StsTestClient(_pipes.Client).LoginAsync("Alice@Example.COM", "correct horse");
        Assert.Null(error);
        Assert.NotNull(token);
    }

    [Fact]
    public async Task RequestsInOneWriteAreAllHandledInWorkThatGrowsLinearlyWithTheirCount()
    {
        long fewer = await PingsThenLoginStartAsync(1000, 50030);
        long more = await PingsThenLoginStartAsync(4000, 50031);

        // Four times the requests: linear work allocates about four times as much, and copying the unread rest after each request
        // about sixteen times.
        Assert.True(more < 6 * fewer, $"{fewer} bytes allocated for 1000 pings, {more} for 4000.");
    }

    [Fact]
    public async Task RequestsBehindKeyDataInOneLargeWriteAreEachDecryptedOnce()
    {
        var client = new StsTestClient(_pipes.Client);
        (byte[] salt, byte[] b) = await LoginStartAsync(client, "alice@example.com");
        var srp = new Srp6Client("alice@example.com", "correct horse");
        (byte[] a, byte[] m1) = srp.Answer(salt, b);

        // KeyData in clear, then pings and LoginFinish under the new key: more bytes than the server takes from the transport at once.
        List<(string Uri, string Body, bool KeyOnAfter, byte[]? Key)> requests =
        [
            ("/Auth/KeyData", $"<Request>\n<KeyData>{StsTestClient.WriteTwo(a, m1)}</KeyData>\n</Request>\n", true, srp.SessionKey),
        ];
        for (int i = 0; i < 2000; i++)
            requests.Add(("/Sts/Ping", "<Ping>\n</Ping>\n", false, null));
        requests.Add(("/Auth/LoginFinish", "", false, null));
        await client.SendTogetherAsync([.. requests]);

        Assert.Equal(200, (await client.ReceiveAsync()).Status);
        client.ReceiveKeyOn(srp.SessionKey);
        StsReply finish = await client.ReceiveAsync();
        Assert.Equal(200, finish.Status);
        Assert.Contains("<UserName>", finish.Body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task AnUnknownUriIsLoggedAtDebugWithItsMethodAndUriAndNeverItsBody()
    {
        var logger = new RecordingLogger<StsConnection>();
        var pipes = new PipePair();
        Serve(pipes, 50050, logger: logger);

        var client = new StsTestClient(pipes.Client);
        await client.SendAsync("/Sts/Mystery", "<Request>\n<Marker>body-marker-7741</Marker>\n</Request>\n");
        Assert.True(await client.IsClosedAsync(s_wait));

        string[] debug = [.. logger.Entries.Where(entry => entry.StartsWith("Debug ", StringComparison.Ordinal))];
        string line = Assert.Single(debug);
        Assert.Contains("POST", line, StringComparison.Ordinal);
        Assert.Contains("/Sts/Mystery", line, StringComparison.Ordinal);
        Assert.DoesNotContain(logger.Entries, entry => entry.Contains("body-marker-7741", StringComparison.Ordinal));
        Assert.Contains(logger.Entries, entry => entry.StartsWith("Warning ", StringComparison.Ordinal)
            && entry.Contains("STS request (unknown) refused in state New", StringComparison.Ordinal));
    }

    [Fact]
    public async Task AKnownUriOutOfItsStateIsLoggedAtDebugToo()
    {
        var logger = new RecordingLogger<StsConnection>();
        var pipes = new PipePair();
        Serve(pipes, 50051, logger: logger);

        var client = new StsTestClient(pipes.Client);
        await client.SendAsync("/Auth/LoginFinish", "");
        Assert.True(await client.IsClosedAsync(s_wait));
        Assert.Contains(logger.Entries, entry => entry.StartsWith("Debug ", StringComparison.Ordinal)
            && entry.Contains("/Auth/LoginFinish", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ALoggedUriKeepsPrintableAsciiOnlyAndIsCutAtEightyCharacters()
    {
        var logger = new RecordingLogger<StsConnection>();
        var pipes = new PipePair();
        Serve(pipes, 50052, logger: logger);

        string uri = "/A\u0001B\u007FC\tD" + new string('x', 200);
        var client = new StsTestClient(pipes.Client);
        await client.SendAsync(uri, "<Request>\n</Request>\n");
        Assert.True(await client.IsClosedAsync(s_wait));

        string line = Assert.Single(logger.Entries, entry => entry.StartsWith("Debug ", StringComparison.Ordinal));
        Assert.Contains("/A?B?C?D" + new string('x', 72), line, StringComparison.Ordinal);
        Assert.DoesNotContain(new string('x', 73), line, StringComparison.Ordinal);
        Assert.DoesNotContain('\u0001', line);
        Assert.DoesNotContain('\u007F', line);
        Assert.DoesNotContain('\t', line);
    }

    [Fact]
    public void NonAsciiTextInALoggedUriBecomesQuestionMarks()
    {
        Assert.Equal("/a?b?c", StsConnection.SanitiseForLog("/aéb中c"));
        Assert.Equal(80, StsConnection.SanitiseForLog(new string('é', 500)).Length);
    }

    [Fact]
    public async Task NothingSecretIsEverLogged()
    {
        _directory.Add("bob@example.com", "hunter2-correct");
        AccountCredentials bob = (await _directory.FindAsync("bob@example.com", TestContext.Current.CancellationToken))!;
        var logger = new RecordingLogger<StsConnection>();
        var granted = new PipePair();
        var wrong = new PipePair();
        var unknown = new PipePair();
        var garbage = new PipePair();
        Serve(granted, 50020, logger: logger);
        Serve(wrong, 50021, logger: logger);
        Serve(unknown, 50022, logger: logger);
        Serve(garbage, 50023, logger: logger);

        var grantedClient = new StsTestClient(granted.Client);
        (Guid? token, _) = await grantedClient.LoginAsync("bob@example.com", "hunter2-correct");
        await new StsTestClient(wrong.Client).LoginAsync("bob@example.com", "hunter2-wrong");
        await new StsTestClient(unknown.Client).LoginAsync("ghost@example.com", "hunter2-ghost");
        var garbageClient = new StsTestClient(garbage.Client);
        await garbageClient.SendAsync("/Sts/Connect", "garbage");
        Assert.True(await garbageClient.IsClosedAsync(s_wait));

        (byte[] ghostSalt, byte[] ghostVerifier) = new UnknownAccountCredentials(_options).For("ghost@example.com");
        byte[][] secrets = [bob.Salt, bob.Verifier, ghostSalt, ghostVerifier, grantedClient.SessionKey!];
        List<string> forbidden =
        [
            "hunter2",
            token!.Value.ToString(),
            token.Value.ToString("N"),
            .. secrets.Select(s => Convert.ToBase64String(s)),
            .. secrets.Select(s => Convert.ToHexString(s)),
            .. secrets.Select(s => Convert.ToHexString(s).ToLowerInvariant()),
        ];

        IReadOnlyList<string> entries = logger.Entries;
        Assert.NotEmpty(entries);
        foreach (string entry in entries)
        {
            foreach (string secret in forbidden)
                Assert.DoesNotContain(secret, entry, StringComparison.OrdinalIgnoreCase);
        }
    }

    private void Serve(PipePair pipes, int port, LoginRateLimiter? limiter = null, ILogger<StsConnection>? logger = null) =>
        Serve(pipes.Server, port, limiter, logger);

    private void Serve(IDuplexPipe server, int port, LoginRateLimiter? limiter = null, ILogger<StsConnection>? logger = null) =>
        _runs.Add(NewConnection(limiter, logger).RunAsync(server, new IPEndPoint(IPAddress.Loopback, port), _stop.Token));

    private StsConnection NewConnection(LoginRateLimiter? limiter = null, ILogger<StsConnection>? logger = null) =>
        new(_directory, _persistence, _accounts, new UnknownAccountCredentials(_options), limiter ?? new LoginRateLimiter(_time, _options),
            _options, _time, logger ?? NullLogger<StsConnection>.Instance);

    private IDuplexPipe OpenSecond()
    {
        var pipes = new PipePair();
        Serve(pipes, 50001);
        return pipes.Client;
    }

    private static async Task<byte[]> LoginStartSaltAsync(StsTestClient client, string name) => (await LoginStartAsync(client, name)).Salt;

    private static async Task<(byte[] Salt, byte[] B)> LoginStartAsync(StsTestClient client, string name, bool connect = true)
    {
        if (connect)
            await client.SendAsync("/Sts/Connect", Connect);
        await client.SendAsync("/Auth/LoginStart", $"<Request>\n<LoginName>{name}</LoginName>\n</Request>\n");
        return StsTestClient.ReadTwo((await client.ReceiveAsync()).Xml().GetElementsByTagName("KeyData")[0]!.InnerText);
    }

    // The pings, then Connect and LoginStart, all written before the run starts, so the server reads them without waiting. The run's
    // allocations are counted on this thread, which holds only while the run completes without yielding; the test checks that.
    private async Task<long> PingsThenLoginStartAsync(int pings, int port)
    {
        var requests = new StringBuilder();
        for (int i = 0; i < pings; i++)
            requests.Append(Request("/Sts/Ping", "<Ping>\n</Ping>\n"));
        requests.Append(Request("/Sts/Connect", Connect));
        requests.Append(Request("/Auth/LoginStart", "<Request>\n<LoginName>alice@example.com</LoginName>\n</Request>\n"));
        var pipes = new PipePair();
        pipes.Client.Output.Write(Encoding.ASCII.GetBytes(requests.ToString()));
        await pipes.Client.Output.CompleteAsync();
        StsConnection connection = NewConnection();

        long before = GC.GetAllocatedBytesForCurrentThread();
        Task run = connection.RunAsync(pipes.Server, new IPEndPoint(IPAddress.Loopback, port), _stop.Token);
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.True(run.IsCompletedSuccessfully, "The run yielded, so its allocations were not all counted on this thread.");
        Assert.Single((await new StsTestClient(pipes.Client).ReceiveAsync()).Xml().GetElementsByTagName("KeyData"));
        return allocated;
    }

    private static string Request(string uri, string body) => $"POST {uri} STS/1.0\r\nl:{body.Length}\r\ns:1\r\n\r\n{body}";

    // Connect, LoginStart and a KeyData with the given password, leaving the server's answer to the proof unread.
    private static async Task ProveAsync(StsTestClient client, string name, string password)
    {
        (byte[] salt, byte[] b) = await LoginStartAsync(client, name);
        (byte[] a, byte[] m1) = new Srp6Client(name.ToLowerInvariant(), password).Answer(salt, b);
        await client.SendAsync("/Auth/KeyData", $"<Request>\n<KeyData>{StsTestClient.WriteTwo(a, m1)}</KeyData>\n</Request>\n");
    }

    private sealed record Transport(PipeReader Input, PipeWriter Output) : IDuplexPipe;

    // Passes every write through. Once armed, it steps the clock inside the next flush, before the bytes reach the client: the step
    // lands after the server has written a reply and before the server's next line runs.
    private sealed class ClockStepWriter : PipeWriter
    {
        private readonly PipeWriter _inner;
        private readonly FakeTimeProvider _time;
        private long _stepTicks;

        public ClockStepWriter(PipeWriter inner, FakeTimeProvider time)
        {
            _inner = inner;
            _time = time;
        }

        public void StepOnNextFlush(TimeSpan step) => Interlocked.Exchange(ref _stepTicks, step.Ticks);

        public override void Advance(int bytes) => _inner.Advance(bytes);

        public override void CancelPendingFlush() => _inner.CancelPendingFlush();

        public override void Complete(Exception? exception = null) => _inner.Complete(exception);

        public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default)
        {
            long step = Interlocked.Exchange(ref _stepTicks, 0);
            if (step > 0)
                _time.Advance(TimeSpan.FromTicks(step));
            return _inner.FlushAsync(cancellationToken);
        }

        public override Memory<byte> GetMemory(int sizeHint = 0) => _inner.GetMemory(sizeHint);

        public override Span<byte> GetSpan(int sizeHint = 0) => _inner.GetSpan(sizeHint);
    }

    private sealed class Directory : IAccountDirectory
    {
        private readonly Dictionary<string, AccountCredentials> _accounts = new(StringComparer.OrdinalIgnoreCase);
        private int _reads;

        public int Reads => Volatile.Read(ref _reads);

        public bool Fail { get; set; }

        public void Add(string email, string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            _accounts[email] = new AccountCredentials(_accounts.Count + 1, email, salt, Srp6Provider.GenerateVerifier(salt, email, password), null, false);
        }

        public Task<AccountCredentials?> FindAsync(string email, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _reads);
            return Fail
                ? throw new InvalidOperationException("the directory is down")
                : Task.FromResult(_accounts.GetValueOrDefault(email));
        }
    }
}
