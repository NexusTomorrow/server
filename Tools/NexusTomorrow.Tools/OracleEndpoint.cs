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
/// <see cref="OracleOptions.TokenFile"/> at this start, within <see cref="FirstLineTime"/> and <see cref="FirstLineChars"/>; the
/// connection closes on the first line that does not parse or authenticate. Each later line is a request (<see cref="OracleVerbs"/>),
/// answered in order; a tap's records interleave with the answers. A connection with <see cref="OutboxLines"/> lines waiting, an answer
/// or a tap record, is closed: its socket is closed and its taps detach.
/// </summary>
public sealed partial class OracleEndpoint : BackgroundService
{
    private readonly OracleOptions _options;
    private readonly OracleVerbs _verbs;
    private readonly ILogger<OracleEndpoint> _logger;
    private byte[] _token = [];

    /// <summary>
    /// How long a connection has to send its token line.
    /// </summary>
    internal TimeSpan FirstLineTime { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The longest token line read, in characters; the token line itself is under a hundred.
    /// </summary>
    internal int FirstLineChars { get; init; } = 4096;

    /// <summary>
    /// The lines a connection may have waiting to be written to it.
    /// </summary>
    internal int OutboxLines { get; init; } = 1 << 16;

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
        await WriteTokenAsync(_options.TokenFile, token, stoppingToken);
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

    // The token file sits under the user's local application data, which Windows keeps to the user; elsewhere it is created anew for
    // its owner alone (CreateNew, so a file that reappears after the delete fails the start rather than keep its old mode).
    private static async Task WriteTokenAsync(string path, string token, CancellationToken cancellationToken)
    {
        var options = new FileStreamOptions { Mode = FileMode.Create, Access = FileAccess.Write };
        if (!OperatingSystem.IsWindows())
        {
            File.Delete(path);
            options.Mode = FileMode.CreateNew;
            options.UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;
        }

        await using var file = new FileStream(path, options);
        await file.WriteAsync(Encoding.ASCII.GetBytes(token), cancellationToken);
    }

    private async Task ServeAsync(TcpClient client, CancellationToken stoppingToken)
    {
        using (client)
        {
            NetworkStream stream = client.GetStream();
            using var reader = new StreamReader(stream, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var outbox = Channel.CreateBounded<string>(new BoundedChannelOptions(OutboxLines) { SingleReader = true, FullMode = BoundedChannelFullMode.Wait });

            // The connection's life: the host stopping, a refusal, or a full outbox (a reply, or a tap record through
            // OracleConnection.Send) ends it, and the socket is closed before the writer is awaited, so a peer that stops reading
            // cannot hold it open.
            var lifetime = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            bool overflowed = false;
            var connection = new OracleConnection(outbox.Writer, full: () =>
            {
                overflowed = true;
                Cancel(lifetime);
            });
            Task writer = WriteLinesAsync(stream, outbox.Reader, lifetime.Token);
            try
            {
                if (await FirstLineAsync(reader, lifetime.Token) is not { } first || !Authenticates(first))
                {
                    Closed("its first line was not the token");
                    return;
                }

                while (await reader.ReadLineAsync(lifetime.Token) is { } line)
                {
                    if (Parse(line) is not { } request)
                    {
                        Closed("it sent a line that is not a JSON object");
                        return;
                    }

                    JsonObject reply = await _verbs.HandleAsync(request, connection, lifetime.Token);
                    if (!connection.Send(reply.ToJsonString()))
                        return;
                }
            }
            catch (Exception exception) when (exception is IOException or OperationCanceledException or ObjectDisposedException)
            {
            }
            catch (Exception exception)
            {
                ConnectionFailed(exception);
            }
            finally
            {
                if (overflowed)
                    Closed($"it left {OutboxLines} lines unread");

                // Taps first, so none writes to an outbox that has completed.
                connection.Dispose();
                outbox.Writer.TryComplete();
                Cancel(lifetime);
                client.Close();
                await writer;
                lifetime.Dispose();
            }
        }
    }

    // A tap can report a full outbox from another thread after the connection has ended and disposed its source.
    private static void Cancel(CancellationTokenSource lifetime)
    {
        try
        {
            lifetime.Cancel();
        }
        catch (ObjectDisposedException)
        {
        }
    }

    // The token line, or null when it does not arrive whole within the limits; read before the connection is trusted.
    private async Task<string?> FirstLineAsync(StreamReader reader, CancellationToken stoppingToken)
    {
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        deadline.CancelAfter(FirstLineTime);
        var line = new StringBuilder();
        char[] one = new char[1];
        try
        {
            while (await reader.ReadAsync(one, deadline.Token) == 1)
            {
                if (one[0] == '\n')
                    return line.ToString().TrimEnd('\r');
                if (line.Length == FirstLineChars)
                    return null;
                line.Append(one[0]);
            }
        }
        catch (OperationCanceledException) when (!stoppingToken.IsCancellationRequested)
        {
        }

        return null;
    }

    private bool Authenticates(string line)
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

    [LoggerMessage(EventId = 9002, Level = LogLevel.Warning, Message = "The oracle endpoint closed a connection: {Reason}")]
    private partial void Closed(string reason);

    [LoggerMessage(EventId = 9003, Level = LogLevel.Error, Message = "The oracle endpoint dropped a connection on an error it did not expect")]
    private partial void ConnectionFailed(Exception exception);
}
