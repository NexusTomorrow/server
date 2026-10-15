using System.Text;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// Builds generated source with four-space indentation and CRLF line endings.
/// </summary>
internal sealed class SourceWriter
{
    private readonly StringBuilder _text = new();
    private int _indent;

    public SourceWriter Line(string line = "")
    {
        if (line.Length > 0)
            _text.Append(' ', _indent * 4).Append(line);
        _text.Append("\r\n");
        return this;
    }

    public SourceWriter Open(string line)
    {
        Line(line);
        Line("{");
        _indent++;
        return this;
    }

    public SourceWriter Close(string closing = "}")
    {
        _indent--;
        Line(closing);
        return this;
    }

    public override string ToString()
    {
        return _text.ToString();
    }
}
