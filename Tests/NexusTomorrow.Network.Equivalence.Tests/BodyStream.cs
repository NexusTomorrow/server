namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// A read-only body that throws on a read past its end. A <see cref="MemoryStream"/> returns -1 there, which <see cref="GamePacketReader"/> takes as the byte
/// 0xFF and reads on, so a legacy reader that wants more bits than the body holds would otherwise pass with invented ones.
/// </summary>
internal sealed class BodyStream : Stream
{
    private readonly MemoryStream _body;

    public BodyStream(byte[] bytes)
    {
        _body = new MemoryStream(bytes, writable: false);
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => _body.Length;

    public override long Position
    {
        get => _body.Position;
        set => _body.Position = value;
    }

    public override int ReadByte()
    {
        return _body.Position < _body.Length ? _body.ReadByte() : throw new EndOfStreamException($"The legacy reader read past the end of the {_body.Length}-byte body.");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (count > _body.Length - _body.Position)
            throw new EndOfStreamException($"The legacy reader read past the end of the {_body.Length}-byte body.");
        return _body.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _body.Seek(offset, origin);
    }

    public override void Flush()
    {
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException("The body is read-only.");
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException("The body is read-only.");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _body.Dispose();
        base.Dispose(disposing);
    }
}
