using System.Buffers;
using System.Text;
using NexusTomorrow.Gateway.Sts;

namespace NexusTomorrow.Gateway.Tests.Sts;

public sealed class StsRequestReaderTests
{
    private static readonly byte[] s_request = Encoding.UTF8.GetBytes("POST /Auth/LoginStart STS/1.0\r\nl:5\r\ns:3\r\n\r\nhello");

    [Fact]
    public void AWholeRequestIsRead()
    {
        ReadOnlySequence<byte> buffer = new(s_request);
        Assert.Equal(StsReadStatus.Complete, StsRequestReader.TryRead(ref buffer, out StsRequest? request));
        Assert.Equal("POST", request!.Method);
        Assert.Equal("/Auth/LoginStart", request.Uri);
        Assert.Equal("3", request.Headers["s"]);
        Assert.Equal("hello", request.Body);
        Assert.Equal(0, buffer.Length);
    }

    [Fact]
    public void ARequestFedOneByteAtATimeIsIncompleteUntilWhole()
    {
        for (int length = 0; length < s_request.Length; length++)
        {
            ReadOnlySequence<byte> prefix = new(s_request[..length]);
            Assert.Equal(StsReadStatus.Incomplete, StsRequestReader.TryRead(ref prefix, out _));
        }
    }

    [Fact]
    public void TwoRequestsInOneBufferAreBothRead()
    {
        ReadOnlySequence<byte> buffer = new([.. s_request, .. s_request]);
        Assert.Equal(StsReadStatus.Complete, StsRequestReader.TryRead(ref buffer, out _));
        Assert.Equal(StsReadStatus.Complete, StsRequestReader.TryRead(ref buffer, out _));
        Assert.Equal(0, buffer.Length);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(33)]
    [InlineData(40)]
    [InlineData(41)]
    [InlineData(42)]
    [InlineData(45)]
    public void ARequestSplitAcrossSegmentsReadsTheSameAsOneWhole(int split)
    {
        ReadOnlySequence<byte> buffer = Segments(s_request[..split], s_request[split..]);
        Assert.Equal(StsReadStatus.Complete, StsRequestReader.TryRead(ref buffer, out StsRequest? request));
        Assert.Equal("/Auth/LoginStart", request!.Uri);
        Assert.Equal("3", request.Headers["s"]);
        Assert.Equal("hello", request.Body);
        Assert.Equal(0, buffer.Length);
    }

    [Fact]
    public void ARequestSplitIntoManySegmentsKeepsEachBytePlace()
    {
        byte[][] pieces = [.. s_request.Select(b => new[] { b })];
        ReadOnlySequence<byte> buffer = Segments(pieces);
        Assert.Equal(StsReadStatus.Complete, StsRequestReader.TryRead(ref buffer, out StsRequest? request));
        Assert.Equal("hello", request!.Body);
    }

    [Fact]
    public void ABodyLengthCountsBytesNotCharacters()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("POST /x STS/1.0\r\nl:2\r\n\r\né");
        ReadOnlySequence<byte> buffer = new(bytes);
        Assert.Equal(StsReadStatus.Complete, StsRequestReader.TryRead(ref buffer, out StsRequest? request));
        Assert.Equal("é", request!.Body);
    }

    [Fact]
    public void ABodyShortByItsByteCountStaysIncomplete()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("POST /x STS/1.0\r\nl:3\r\n\r\né");
        ReadOnlySequence<byte> buffer = new(bytes);
        Assert.Equal(StsReadStatus.Incomplete, StsRequestReader.TryRead(ref buffer, out _));
    }

    [Theory]
    [InlineData("POST /x STS/1.0\r\ns:1\r\n\r\n")]                 // no length
    [InlineData("POST /x STS/1.0\r\nl:0\r\nl:0\r\n\r\n")]          // a repeated header
    [InlineData("POST /x\r\nl:0\r\n\r\n")]                          // a request line of two parts
    [InlineData("POST /x STS/1.0\r\nl:99999999\r\n\r\n")]           // a body over the cap
    [InlineData("POST /x STS/1.0\r\nl:16385\r\n\r\n")]              // one byte over the body cap
    [InlineData("POST /x STS/1.0\r\nl:-1\r\n\r\n")]                 // a negative length
    [InlineData("POST /x STS/1.0\r\nl:abc\r\n\r\n")]                // a length that is not a number
    [InlineData("POST /x STS/1.0\r\nnocolon\r\n\r\n")]              // a header without a colon
    public void AMalformedHeadIsRefused(string head)
    {
        ReadOnlySequence<byte> buffer = new(Encoding.UTF8.GetBytes(head));
        Assert.Equal(StsReadStatus.Malformed, StsRequestReader.TryRead(ref buffer, out _));
    }

    [Fact]
    public void ABodyAtTheCapIsRefusedOnlyOnceItIsOver()
    {
        ReadOnlySequence<byte> buffer = new(Encoding.UTF8.GetBytes($"POST /x STS/1.0\r\nl:{StsRequestReader.MaxBodyBytes}\r\n\r\n"));
        Assert.Equal(StsReadStatus.Incomplete, StsRequestReader.TryRead(ref buffer, out _));
    }

    [Fact]
    public void AHeadPastTheCapWithoutItsEndIsRefused()
    {
        ReadOnlySequence<byte> buffer = new(new byte[StsRequestReader.MaxHeaderBytes + 1]);
        Assert.Equal(StsReadStatus.Malformed, StsRequestReader.TryRead(ref buffer, out _));
    }

    [Fact]
    public void AHeadPastTheCapWithItsEndIsRefused()
    {
        string head = "POST /x STS/1.0\r\nx:" + new string('a', StsRequestReader.MaxHeaderBytes) + "\r\nl:0\r\n\r\n";
        ReadOnlySequence<byte> buffer = new(Encoding.UTF8.GetBytes(head));
        Assert.Equal(StsReadStatus.Malformed, StsRequestReader.TryRead(ref buffer, out _));
    }

    private static ReadOnlySequence<byte> Segments(params byte[][] pieces)
    {
        Segment? first = null;
        Segment? last = null;
        foreach (byte[] piece in pieces)
        {
            last = last is null ? new Segment(piece, 0) : last.Append(piece);
            first ??= last;
        }

        return new ReadOnlySequence<byte>(first!, 0, last!, last!.Memory.Length);
    }

    private sealed class Segment : ReadOnlySequenceSegment<byte>
    {
        public Segment(byte[] bytes, long runningIndex)
        {
            Memory = bytes;
            RunningIndex = runningIndex;
        }

        public Segment Append(byte[] bytes)
        {
            var next = new Segment(bytes, RunningIndex + Memory.Length);
            Next = next;
            return next;
        }
    }
}
