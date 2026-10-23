using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Tools;

/// <summary>
/// The oracle's endpoint: loopback TCP, one JSON object per line each way. A connection's first line must carry the token written to
/// <see cref="OracleOptions.TokenFile"/> at this start; the connection closes on the first line that does not parse or authenticate.
/// Each later line is a request (<see cref="OracleVerbs"/>), answered in order; a tap's records interleave with the answers.
/// </summary>
public sealed partial class OracleEndpoint : BackgroundService
{
    private readonly OracleOptions _options;
    private readonly OracleVerbs _verbs;
    private readonly ILogger<OracleEndpoint> _logger;
    private byte[] _token = [];

    /// <summary>
    /// Create the endpoint.
    /// </summary>
    public OracleEndpoint(IOptions<OracleOptions> options, OracleVerbs verbs, ILogger<OracleEndpoint> logger)
    {
        _options = options.Value;
        _verbs = verbs;
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
            return;

        string token = RandomNumberGenerator.GetHexString(64, lowercase: true);
        Directory.CreateDirectory(Path.GetDirectoryName(_options.TokenFile)!);
        await File.WriteAllTextAsync(_options.TokenFile, token, stoppingToken);
        _token = Encoding.ASCII.GetBytes(token);

        var listener = new TcpListener(IPAddress.Parse(_options.Address), _options.Port);
        listener.Start();
        Listening(listener.LocalEndpoint);
        try
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(stoppingToken);
                _ = ServeAsync(client, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task ServeAsync(TcpClient client, CancellationToken stoppingToken)
    {
        using (client)
        {
            NetworkStream stream = client.GetStream();
            using var reader = new StreamReader(stream, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            Channel<string> outbox = Channel.CreateUnbounded<string>(new UnboundedChannelOptions { SingleReader = true });
            using var connection = new OracleConnection(outbox.Writer);
            Task writer = WriteLinesAsync(stream, outbox.Reader, stoppingToken);
            try
            {
                if (!Authenticates(await reader.ReadLineAsync(stoppingToken)))
                {
                    Refused("a first line without the token");
                    return;
                }

                while (await reader.ReadLineAsync(stoppingToken) is { } line)
                {
                    if (Parse(line) is not { } request)
                    {
                        Refused("a line that is not a JSON object");
                        return;
                    }

                    JsonObject reply = await _verbs.HandleAsync(request, connection, stoppingToken);
                    outbox.Writer.TryWrite(reply.ToJsonString());
                }
            }
            catch (Exception exception) when (exception is IOException or OperationCanceledException or ObjectDisposedException)
            {
            }
            finally
            {
                outbox.Writer.TryComplete();
                await writer;
            }
        }
    }

    private bool Authenticates(string? line)
    {
        if (Parse(line) is not { } first || first["token"] is not JsonValue value || !value.TryGetValue(out string? token))
            return false;

        byte[] given = Encoding.ASCII.GetBytes(token);
        return given.Length == _token.Length && CryptographicOperations.FixedTimeEquals(given, _token);
    }

    private static JsonObject? Parse(string? line)
    {
        if (line is null)
            return null;

        try
        {
            return JsonNode.Parse(line) as JsonObject;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static async Task WriteLinesAsync(Stream stream, ChannelReader<string> lines, CancellationToken stoppingToken)
    {
        try
        {
            await foreach (string line in lines.ReadAllAsync(stoppingToken))
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes(line + "\n"), stoppingToken);
                await stream.FlushAsync(stoppingToken);
            }
        }
        catch (Exception exception) when (exception is IOException or OperationCanceledException or ObjectDisposedException)
        {
        }
    }

    [LoggerMessage(EventId = 9001, Level = LogLevel.Warning, Message = "The oracle endpoint is listening on {Endpoint}")]
    private partial void Listening(EndPoint endpoint);

    [LoggerMessage(EventId = 9002, Level = LogLevel.Warning, Message = "The oracle endpoint closed a connection that sent {Reason}")]
    private partial void Refused(string reason);
}
