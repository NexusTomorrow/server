using System.Buffers;
using System.Globalization;
using System.Text;

namespace NexusTomorrow.Gateway.Sts;

/// <summary>
/// Reads STS requests (<c>METHOD URI STS/1.0</c>, <c>name:value</c> headers, a blank line, then <c>l</c> bytes of body) from a byte
/// sequence that may be split across any number of segments. The head and body sizes are checked as soon as they can be known, before
/// the bytes behind them are waited for.
/// </summary>
public static class StsRequestReader
{
    /// <summary>
    /// The most bytes a request line, its headers and their closing blank line may take together.
    /// </summary>
    public const int MaxHeaderBytes = 4096;

    /// <summary>
    /// The most bytes a body may take.
    /// </summary>
    public const int MaxBodyBytes = 16384;

    private static readonly UTF8Encoding s_strictUtf8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    /// <summary>
    /// Read one request from the start of <paramref name="buffer"/>. On <see cref="StsReadStatus.Complete"/> the request's bytes are
    /// consumed from <paramref name="buffer"/>; otherwise it is left as it was.
    /// </summary>
    public static StsReadStatus TryRead(ref ReadOnlySequence<byte> buffer, out StsRequest? request)
    {
        request = null;
        var reader = new SequenceReader<byte>(buffer);
        if (!reader.TryReadTo(out ReadOnlySequence<byte> head, "\r\n\r\n"u8))
        {
            // The blank line would end past the cap.
            return buffer.Length >= MaxHeaderBytes ? StsReadStatus.Malformed : StsReadStatus.Incomplete;
        }

        if (head.Length + 4 > MaxHeaderBytes)
            return StsReadStatus.Malformed;

        // StsConnection reads from one array, so the head is read where it lies; a head split across segments is copied once.
        ReadOnlySpan<byte> headBytes = head.IsSingleSegment ? head.FirstSpan : head.ToArray();
        if (!Ascii.IsValid(headBytes))
            return StsReadStatus.Malformed;

        string[] lines = Encoding.ASCII.GetString(headBytes).Split("\r\n");
        string[] requestLine = lines[0].Split(' ');
        if (requestLine.Length != 3 || requestLine.Any(part => part.Length == 0))
            return StsReadStatus.Malformed;

        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string line in lines.Skip(1))
        {
            int colon = line.IndexOf(':', StringComparison.Ordinal);
            if (colon <= 0 || !headers.TryAdd(line[..colon], line[(colon + 1)..]))
                return StsReadStatus.Malformed;
        }

        if (!headers.TryGetValue("l", out string? lengthText)
            || !int.TryParse(lengthText, NumberStyles.None, CultureInfo.InvariantCulture, out int length)
            || length > MaxBodyBytes)
        {
            return StsReadStatus.Malformed;
        }

        if (reader.Remaining < length)
            return StsReadStatus.Incomplete;

        string body;
        try
        {
            body = s_strictUtf8.GetString(buffer.Slice(reader.Position, length));
        }
        catch (DecoderFallbackException)
        {
            return StsReadStatus.Malformed;
        }

        buffer = buffer.Slice(buffer.GetPosition(length, reader.Position));
        request = new StsRequest(requestLine[0], requestLine[1], headers, body);
        return StsReadStatus.Complete;
    }
}
