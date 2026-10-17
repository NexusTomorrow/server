using System.Buffers;
using System.Globalization;
using System.IO.Pipelines;
using System.Text;
using System.Xml;
using NexusTomorrow.Cryptography;

namespace NexusTomorrow.Testing;

public sealed record StsReply(int Status, IReadOnlyDictionary<string, string> Headers, string Body)
{
    public XmlDocument Xml()
    {
        var document = new XmlDocument();
        document.LoadXml(Body);
        return document;
    }
}

public sealed class StsTestClient
{
    private readonly IDuplexPipe _pipe;
    private Arc4Provider? _send;
    private Arc4Provider? _receive;
    private int _sequence;
    private byte[] _pending = [];

    public StsTestClient(IDuplexPipe pipe)
    {
        _pipe = pipe;
    }

    // Write one request; with the key on, its bytes go out RC4-encrypted.
    public async Task SendAsync(string uri, string body)
    {
        await _pipe.Output.WriteAsync(Encode(uri, body));
    }

    // Write bytes exactly as given, for requests that are not well formed. With the key on they go out encrypted.
    public async Task SendRawAsync(byte[] bytes)
    {
        byte[] copy = [.. bytes];
        _send?.Apply(copy);
        await _pipe.Output.WriteAsync(copy);
    }

    // Write several requests in one write, to put them in one read on the server side.
    public async Task SendTogetherAsync(params (string Uri, string Body, bool KeyOnAfter, byte[]? Key)[] requests)
    {
        var all = new List<byte>();
        foreach ((string uri, string body, bool keyOnAfter, byte[]? key) in requests)
        {
            all.AddRange(Encode(uri, body));
            if (keyOnAfter && key is not null)
                _send = new Arc4Provider(key);
        }

        await _pipe.Output.WriteAsync(all.ToArray());
    }

    // The session key K once LoginAsync has passed the proof; null before.
    public byte[]? SessionKey { get; private set; }

    public void KeyOn(byte[] key)
    {
        _send = new Arc4Provider(key);
        ReceiveKeyOn(key);
    }

    // Bytes already buffered past the last reply arrived under the new key, so they are decrypted now.
    public void ReceiveKeyOn(byte[] key)
    {
        _receive = new Arc4Provider(key);
        _receive.Apply(_pending);
    }

    public async Task<StsReply> ReceiveAsync(TimeSpan? timeout = null)
    {
        using var cancel = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(10));
        while (true)
        {
            if (TryParse(out StsReply? reply))
                return reply!;
            ReadResult result = await _pipe.Input.ReadAsync(cancel.Token);
            byte[] chunk = result.Buffer.ToArray();
            _pipe.Input.AdvanceTo(result.Buffer.End);
            _receive?.Apply(chunk);
            _pending = [.. _pending, .. chunk];
            if (result.IsCompleted)
                return TryParse(out reply) ? reply! : throw new EndOfStreamException("The STS closed the connection.");
        }
    }

    public async Task<bool> IsClosedAsync(TimeSpan timeout)
    {
        using var cancel = new CancellationTokenSource(timeout);
        try
        {
            while (true)
            {
                ReadResult result = await _pipe.Input.ReadAsync(cancel.Token);
                _pipe.Input.AdvanceTo(result.Buffer.End);
                if (result.IsCompleted)
                    return true;
            }
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    // The whole login: returns the game token the STS issued, or the error reply that stopped it.
    public async Task<(Guid? Token, StsReply? Error)> LoginAsync(string loginName, string password)
    {
        await SendAsync("/Sts/Connect", "<Connect>\n<ConnType>400</ConnType>\n</Connect>\n");
        await SendAsync("/Auth/LoginStart", $"<Request>\n<LoginName>{loginName}</LoginName>\n</Request>\n");
        StsReply start = await ReceiveAsync();
        if (start.Status != 200)
            return (null, start);
        (byte[] salt, byte[] b) = ReadTwo(start.Xml().GetElementsByTagName("KeyData")[0]!.InnerText);

        var srp = new Srp6Client(loginName.ToLowerInvariant(), password);
        (byte[] a, byte[] m1) = srp.Answer(salt, b);
        await SendAsync("/Auth/KeyData", $"<Request>\n<KeyData>{WriteTwo(a, m1)}</KeyData>\n</Request>\n");
        _send = new Arc4Provider(srp.SessionKey);
        StsReply proof = await ReceiveAsync();
        if (proof.Status != 200)
            return (null, proof);
        ReceiveKeyOn(srp.SessionKey);
        SessionKey = srp.SessionKey;

        await SendAsync("/Auth/LoginFinish", "");
        await ReceiveAsync();
        await SendAsync("/Auth/RequestGameToken", "");
        StsReply token = await ReceiveAsync();
        return (Guid.Parse(token.Xml().GetElementsByTagName("Token")[0]!.InnerText), null);
    }

    public static (byte[] First, byte[] Second) ReadTwo(string base64)
    {
        using var reader = new BinaryReader(new MemoryStream(Convert.FromBase64String(base64)));
        byte[] first = reader.ReadBytes(reader.ReadInt32());
        byte[] second = reader.ReadBytes(reader.ReadInt32());
        return (first, second);
    }

    public static string WriteTwo(byte[] first, byte[] second)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write(first.Length);
        writer.Write(first);
        writer.Write(second.Length);
        writer.Write(second);
        writer.Flush();
        return Convert.ToBase64String(stream.ToArray());
    }

    private byte[] Encode(string uri, string body)
    {
        byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
        byte[] bytes = [.. Encoding.ASCII.GetBytes($"POST {uri} STS/1.0\r\nl:{bodyBytes.Length}\r\ns:{++_sequence}\r\n\r\n"), .. bodyBytes];
        _send?.Apply(bytes);
        return bytes;
    }

    private bool TryParse(out StsReply? reply)
    {
        reply = null;
        string text = Encoding.UTF8.GetString(_pending);
        int end = text.IndexOf("\r\n\r\n", StringComparison.Ordinal);
        if (end < 0)
            return false;
        string[] lines = text[..end].Split("\r\n");
        var headers = lines.Skip(1).Select(l => l.Split(':', 2)).ToDictionary(p => p[0], p => p[1]);
        int length = int.Parse(headers["l"], CultureInfo.InvariantCulture);
        int headerBytes = Encoding.UTF8.GetByteCount(text[..(end + 4)]);
        if (_pending.Length < headerBytes + length)
            return false;
        string body = Encoding.UTF8.GetString(_pending, headerBytes, length);
        _pending = _pending[(headerBytes + length)..];
        reply = new StsReply(int.Parse(lines[0].Split(' ')[1], CultureInfo.InvariantCulture), headers, body);
        return true;
    }
}
