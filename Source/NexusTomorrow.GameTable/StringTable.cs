using System.Text;

namespace NexusTomorrow.GameTable;

public class StringTable : IDisposable
{
    private readonly MemoryStream _stream;
    private readonly BinaryReader _reader;

    public StringTable(byte[] buffer)
    {
        _stream = new MemoryStream(buffer);
        _reader = new BinaryReader(_stream, Encoding.Unicode);
    }

    public void Dispose()
    {
        _reader.Dispose();
        _stream.Dispose();
    }

    public string GetEntry(uint offset)
    {
        _stream.Position = offset;
        return _reader.ReadWideString();
    }
}
