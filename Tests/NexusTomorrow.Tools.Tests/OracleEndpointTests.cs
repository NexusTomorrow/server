using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Tools.Tests;

public sealed class OracleEndpointTests : IAsyncLifetime
{
    private static readonly TimeSpan s_wait = TimeSpan.FromSeconds(10);

    private readonly string _folder = Directory.CreateTempSubdirectory("oracle-endpoint-").FullName;
    private readonly AccountOwner _accounts =
        new(new FakeTimeProvider(), Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, new FakeWriteBehind(), FixedEntitlementCaps.Defaults());
    private OracleEndpoint? _endpoint;

    private string TokenFile => Path.Combine(_folder, "token");

    public ValueTask InitializeAsync()
    {
        _accounts.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_endpoint is not null)
            await _endpoint.StopAsync(CancellationToken.None);
        await _accounts.StopAsync().WaitAsync(s_wait);
        Directory.Delete(_folder, recursive: true);
    }

    [Fact]
    public async Task AnEndpointThatIsOffListensNowhereAndWritesNoToken()
    {
        int port = await StartAsync(enabled: false);

        await Assert.ThrowsAnyAsync<SocketException>(async () =>
        {
            using var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, port, TestContext.Current.CancellationToken);
        });
        Assert.False(File.Exists(TokenFile));
    }

    [Fact]
    public async Task TheTokenOpensTheConnectionAndAnAnswerCarriesItsRequestsId()
    {
        int port = await StartAsync(enabled: true);
        using LineClient client = await LineClient.ConnectAsync(port);

        await client.SendAsync(new JsonObject { ["token"] = await TokenAsync() });
        await client.SendAsync(new JsonObject { ["id"] = 7, ["verb"] = "status" });

        JsonObject reply = Assert.IsType<JsonObject>(await client.ReadAsync());
        Assert.Equal(7, (int)reply["id"]!);
        Assert.True((bool)reply["ok"]!);
        Assert.Equal(0, (int)reply["online"]!);
    }

    [Fact]
    public async Task AWrongTokenClosesTheConnectionWithoutAnAnswer()
    {
        int port = await StartAsync(enabled: true);
        await TokenAsync();
        using LineClient client = await LineClient.ConnectAsync(port);

        await client.SendAsync(new JsonObject { ["token"] = new string('0', 64) });
        await client.SendAsync(new JsonObject { ["id"] = 1, ["verb"] = "status" });

        Assert.Null(await client.ReadAsync());
    }

    [Fact]
    public async Task ALineThatIsNotAJsonObjectClosesTheConnection()
    {
        int port = await StartAsync(enabled: true);
        using LineClient client = await LineClient.ConnectAsync(port);

        await client.SendAsync(new JsonObject { ["token"] = await TokenAsync() });
        await client.SendLineAsync("not json");

        Assert.Null(await client.ReadAsync());
    }

    [Fact]
    public async Task AConnectionThatSendsNoTokenInTimeIsClosed()
    {
        int port = await StartAsync(enabled: true, firstLineTime: TimeSpan.FromMilliseconds(200));
        await TokenAsync();
        using LineClient client = await LineClient.ConnectAsync(port);

        Assert.Null(await client.ReadAsync());
    }

    [Fact]
    public async Task AFirstLineLongerThanTheCapIsClosedBeforeItEnds()
    {
        // A deadline past the read's wait, so only the cap can close the connection in time.
        int port = await StartAsync(enabled: true, firstLineTime: TimeSpan.FromMinutes(1));
        await TokenAsync();
        using LineClient client = await LineClient.ConnectAsync(port);

        await client.SendRawAsync(new string('x', 5000));

        Assert.Null(await client.ReadAsync());
    }

    [Fact]
    public async Task APeerThatEndsItsSideStillGetsTheAnswersWaiting()
    {
        int port = await StartAsync(enabled: true);
        using LineClient client = await LineClient.ConnectAsync(port);

        // Many requests in one write, so the end of input reaches the endpoint while answers still wait to be written.
        const int Requests = 3000;
        await client.SendAsync(new JsonObject { ["token"] = await TokenAsync() });
        await client.SendRawAsync(string.Concat(Enumerable.Range(0, Requests).Select(i => $$"""{"id": {{i}}, "verb": "status"}""" + "\n")));
        client.EndSending();

        for (int i = 0; i < Requests; i++)
            Assert.Equal(i, (int)Assert.IsType<JsonObject>(await client.ReadAsync())["id"]!);
        Assert.Null(await client.ReadAsync());
    }

    [Fact]
    public async Task AConnectionThatStopsReadingItsAnswersIsClosed()
    {
        int port = await StartAsync(enabled: true, outboxLines: 4);
        using LineClient client = await LineClient.ConnectAsync(port, receiveBuffer: 1024);
        await client.SendAsync(new JsonObject { ["token"] = await TokenAsync() });

        // A write that blocks past the limit fails the test instead of hanging it.
        using var limit = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        bool closed = false;
        for (int i = 0; i < 200_000 && !closed; i++)
            closed = !await client.TrySendAsync(new JsonObject { ["id"] = i, ["verb"] = "status" }, limit.Token);

        Assert.True(closed);
    }

    [Fact]
    public async Task TheTokenFileIsItsOwnersAlone()
    {
        await StartAsync(enabled: true);
        await TokenAsync();

        Assert.SkipWhen(OperatingSystem.IsWindows(), "Windows keeps local application data to its user");
        if (!OperatingSystem.IsWindows())
            Assert.Equal(UnixFileMode.UserRead | UnixFileMode.UserWrite, File.GetUnixFileMode(TokenFile));
    }

    [Fact]
    public async Task AnUnknownVerbIsAnsweredWithAnError()
    {
        int port = await StartAsync(enabled: true);
        using LineClient client = await LineClient.ConnectAsync(port);

        await client.SendAsync(new JsonObject { ["token"] = await TokenAsync() });
        await client.SendAsync(new JsonObject { ["id"] = 1, ["verb"] = "fly" });

        JsonObject reply = Assert.IsType<JsonObject>(await client.ReadAsync());
        Assert.False((bool)reply["ok"]!);
        Assert.Equal("no verb fly", (string)reply["error"]!);
    }

    private async Task<int> StartAsync(bool enabled, TimeSpan? firstLineTime = null, int outboxLines = 1 << 16)
    {
        int port = FreePort();
        var options = new OracleOptions { Enabled = enabled, Port = port, TokenFile = TokenFile };
        _endpoint = new OracleEndpoint(Options.Create(options), new OracleVerbs(_accounts, TimeProvider.System), NullLogger<OracleEndpoint>.Instance)
        {
            FirstLineTime = firstLineTime ?? TimeSpan.FromSeconds(5),
            OutboxLines = outboxLines,
        };
        await _endpoint.StartAsync(CancellationToken.None);
        return port;
    }

    // The token is written in full before the endpoint listens, so a 64-character file is the whole token.
    private async Task<string> TokenAsync()
    {
        await Eventually.TrueAsync(() => File.Exists(TokenFile) && File.ReadAllText(TokenFile).Length == 64);
        return await File.ReadAllTextAsync(TokenFile, TestContext.Current.CancellationToken);
    }

    private static int FreePort()
    {
        var free = new TcpListener(IPAddress.Loopback, 0);
        free.Start();
        int port = ((IPEndPoint)free.LocalEndpoint).Port;
        free.Stop();
        return port;
    }

    private sealed class LineClient : IDisposable
    {
        private readonly TcpClient _client;
        private readonly StreamReader _reader;
        private readonly Stream _stream;

        private LineClient(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _reader = new StreamReader(_stream, new UTF8Encoding(false));
        }

        // The endpoint starts listening a moment after it writes its token, so the first attempts may be refused.
        public static async Task<LineClient> ConnectAsync(int port, int? receiveBuffer = null)
        {
            for (int attempt = 0; ; attempt++)
            {
                var client = new TcpClient();
                if (receiveBuffer is { } bytes)
                    client.ReceiveBufferSize = bytes;
                try
                {
                    await client.ConnectAsync(IPAddress.Loopback, port);
                    return new LineClient(client);
                }
                catch (SocketException) when (attempt < 50)
                {
                    client.Dispose();
                    await Task.Delay(100);
                }
            }
        }

        public Task SendAsync(JsonObject message) => SendLineAsync(message.ToJsonString());

        public Task SendLineAsync(string line) => SendRawAsync(line + "\n");

        public void EndSending() => _client.Client.Shutdown(SocketShutdown.Send);

        // False once the endpoint has closed the connection.
        public async Task<bool> TrySendAsync(JsonObject message, CancellationToken cancellationToken)
        {
            try
            {
                await _stream.WriteAsync(Encoding.UTF8.GetBytes(message.ToJsonString() + "\n"), cancellationToken);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public async Task SendRawAsync(string text)
        {
            try
            {
                await _stream.WriteAsync(Encoding.UTF8.GetBytes(text));
            }
            catch (IOException)
            {
                // The endpoint may already have closed the connection; the next read reports it.
            }
        }

        public async Task<JsonNode?> ReadAsync()
        {
            try
            {
                string? line = await _reader.ReadLineAsync().WaitAsync(s_wait);
                return line is null ? null : JsonNode.Parse(line);
            }
            catch (IOException)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
            _client.Dispose();
        }
    }
}
