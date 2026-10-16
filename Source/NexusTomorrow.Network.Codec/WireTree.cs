using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace NexusTomorrow.Network.Codec;

/// <summary>
/// The text form of a layout tree, which the generated code and the client facts table both carry. A sequence is nodes separated by one
/// space: a width from 1 to 64 is a field; <c>w</c>, <c>n</c> and <c>t</c> are a wide, a narrow and a terminated wide string; <c>b32</c> is
/// a blob with a 32-bit length and <c>b32a</c> one that starts on a byte; <c>[3:...]</c> is an array counted by the field at index 3 of the
/// same sequence, its element's sequence after the colon; <c>{3:0=(...),2=(...)}</c> is a union selected by the field at index 3, one arm
/// per selector value in ascending order; <c>?</c> is a layout that contains itself.
/// </summary>
public static class WireTree
{
    /// <summary>
    /// Read a tree from its text form.
    /// </summary>
    /// <exception cref="FormatException">The text is not a well-formed tree.</exception>
    public static ImmutableArray<WireNode> Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        int position = 0;
        ImmutableArray<WireNode> nodes = ParseSequence(text, ref position, '\0');
        if (position != text.Length)
            throw Malformed(text, position);
        return nodes;
    }

    /// <summary>
    /// Write a tree in its text form.
    /// </summary>
    public static string Format(ImmutableArray<WireNode> nodes)
    {
        var builder = new StringBuilder();
        AppendSequence(builder, nodes);
        return builder.ToString();
    }

    private static ImmutableArray<WireNode> ParseSequence(string text, ref int position, char end)
    {
        ImmutableArray<WireNode>.Builder nodes = ImmutableArray.CreateBuilder<WireNode>();
        while (position < text.Length && text[position] != end)
        {
            if (nodes.Count > 0)
                Expect(text, ref position, ' ');
            nodes.Add(ParseNode(text, ref position, nodes));
        }

        return nodes.ToImmutable();
    }

    private static WireNode ParseNode(string text, ref int position, ImmutableArray<WireNode>.Builder before)
    {
        if (position >= text.Length)
            throw Malformed(text, position);

        switch (text[position])
        {
            case 'w':
                position++;
                return WireNode.Text(WireNodeKind.WideString);
            case 'n':
                position++;
                return WireNode.Text(WireNodeKind.NarrowString);
            case 't':
                position++;
                return WireNode.Text(WireNodeKind.TerminatedWideString);
            case '?':
                position++;
                return WireNode.Recursive;
            case 'b':
                {
                    position++;
                    int bits = ParseWidth(text, ref position);
                    bool aligned = position < text.Length && text[position] == 'a';
                    if (aligned)
                        position++;
                    return WireNode.Blob(bits, aligned);
                }

            case '[':
                {
                    position++;
                    int count = ParseReference(text, ref position, before);
                    Expect(text, ref position, ':');
                    ImmutableArray<WireNode> element = ParseSequence(text, ref position, ']');
                    Expect(text, ref position, ']');
                    return WireNode.Array(count, element);
                }

            case '{':
                {
                    position++;
                    int selector = ParseReference(text, ref position, before);
                    Expect(text, ref position, ':');
                    ImmutableArray<WireArm>.Builder arms = ImmutableArray.CreateBuilder<WireArm>();
                    while (true)
                    {
                        int start = position;
                        ulong value = ParseUnsigned(text, ref position);
                        if (arms.Count > 0 && value <= arms[^1].Selector)
                            throw Malformed(text, start);
                        Expect(text, ref position, '=');
                        Expect(text, ref position, '(');
                        ImmutableArray<WireNode> nodes = ParseSequence(text, ref position, ')');
                        Expect(text, ref position, ')');
                        arms.Add(new WireArm(value, nodes));
                        if (position >= text.Length || text[position] != ',')
                            break;
                        position++;
                    }

                    Expect(text, ref position, '}');
                    return WireNode.Union(selector, arms.ToImmutable());
                }

            default:
                return WireNode.Field(ParseWidth(text, ref position));
        }
    }

    private static int ParseWidth(string text, ref int position)
    {
        int start = position;
        ulong value = ParseUnsigned(text, ref position);
        if (value is < 1 or > 64)
            throw Malformed(text, start);
        return (int)value;
    }

    // The referenced node must already be in the sequence and be a field: a count or a selector is a field read before what it governs.
    private static int ParseReference(string text, ref int position, ImmutableArray<WireNode>.Builder before)
    {
        int start = position;
        ulong index = ParseUnsigned(text, ref position);
        if (index >= (ulong)before.Count || before[(int)index].Kind != WireNodeKind.Field)
            throw Malformed(text, start);
        return (int)index;
    }

    private static ulong ParseUnsigned(string text, ref int position)
    {
        int start = position;
        ulong value = 0;
        while (position < text.Length && text[position] is >= '0' and <= '9')
        {
            ulong digit = (ulong)(text[position] - '0');
            if (value > (ulong.MaxValue - digit) / 10)
                throw Malformed(text, start);
            value = (value * 10) + digit;
            position++;
        }

        if (position == start)
            throw Malformed(text, start);
        return value;
    }

    private static void Expect(string text, ref int position, char expected)
    {
        if (position >= text.Length || text[position] != expected)
            throw Malformed(text, position);
        position++;
    }

    private static FormatException Malformed(string text, int position)
    {
        return new FormatException(string.Create(CultureInfo.InvariantCulture, $"Not a layout tree at position {position}: \"{text}\"."));
    }

    private static void AppendSequence(StringBuilder builder, ImmutableArray<WireNode> nodes)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i > 0)
                builder.Append(' ');
            AppendNode(builder, nodes[i]);
        }
    }

    private static void AppendNode(StringBuilder builder, WireNode node)
    {
        switch (node.Kind)
        {
            case WireNodeKind.Field:
                builder.Append(node.Bits.ToString(CultureInfo.InvariantCulture));
                break;
            case WireNodeKind.WideString:
                builder.Append('w');
                break;
            case WireNodeKind.NarrowString:
                builder.Append('n');
                break;
            case WireNodeKind.TerminatedWideString:
                builder.Append('t');
                break;
            case WireNodeKind.Recursive:
                builder.Append('?');
                break;
            case WireNodeKind.Blob:
                builder.Append('b').Append(node.Bits.ToString(CultureInfo.InvariantCulture));
                if (node.Aligned)
                    builder.Append('a');
                break;
            case WireNodeKind.Array:
                builder.Append('[').Append(node.Reference.ToString(CultureInfo.InvariantCulture)).Append(':');
                AppendSequence(builder, node.Element);
                builder.Append(']');
                break;
            case WireNodeKind.Union:
                builder.Append('{').Append(node.Reference.ToString(CultureInfo.InvariantCulture)).Append(':');
                for (int i = 0; i < node.Arms.Length; i++)
                {
                    if (i > 0)
                        builder.Append(',');
                    builder.Append(node.Arms[i].Selector.ToString(CultureInfo.InvariantCulture)).Append("=(");
                    AppendSequence(builder, node.Arms[i].Nodes);
                    builder.Append(')');
                }

                builder.Append('}');
                break;
        }
    }
}
