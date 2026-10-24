using System.Buffers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners.Text;

/// <summary>
/// A word filter over a set of rows, each a type and a text. A text is allowed when none of its words matches a row; a word is a
/// maximal run of the client's letters and digits (<see cref="UserTextRules"/>). Both halves ignore case: an exact word by a full
/// case fold, a pattern caseless and culture-invariant.
/// </summary>
public sealed class WordFilter : IWordFilter
{
    // A row's type: an exact word, its text as a pattern, a prefix of a word, a suffix of a word, or anywhere in a word.
    private const uint ExactWord = 0;
    private const uint RawPattern = 1;
    private const uint Prefix = 2;
    private const uint Suffix = 3;
    private const uint Anywhere = 4;

    // A word this long or shorter folds into a buffer on the stack.
    private const int StackFoldLength = 256;

    // A pattern still running after this long counts as a match.
    private static readonly TimeSpan s_matchTimeout = TimeSpan.FromMilliseconds(50);

    private readonly HashSet<string> _words = new(StringComparer.Ordinal);
    private readonly HashSet<string>.AlternateLookup<ReadOnlySpan<char>> _wordsBySpan;
    private readonly Regex[] _patterns;

    /// <summary>
    /// Build the filter from <paramref name="rows"/>. A row of a type the filter does not know, or a pattern row that does not
    /// compile, is logged and skipped.
    /// </summary>
    public WordFilter(IEnumerable<(uint Type, string Text)> rows, ILogger logger)
    {
        var patterns = new List<Regex>();
        int index = 0;
        foreach ((uint type, string text) in rows)
        {
            switch (type)
            {
                case ExactWord:
                    _words.Add(text.ToLowerInvariant());
                    break;
                case RawPattern:
                    try
                    {
                        patterns.Add(Compile(text));
                    }
                    catch (ArgumentException exception)
                    {
                        TextLog.PatternSkipped(logger, index, exception);
                    }

                    break;
                case Prefix:
                    patterns.Add(Compile("^" + Regex.Escape(text)));
                    break;
                case Suffix:
                    patterns.Add(Compile(Regex.Escape(text) + "$"));
                    break;
                case Anywhere:
                    patterns.Add(Compile(Regex.Escape(text)));
                    break;
                default:
                    TextLog.UnknownRowType(logger, index, type);
                    break;
            }

            index++;
        }

        _wordsBySpan = _words.GetAlternateLookup<ReadOnlySpan<char>>();
        _patterns = [.. patterns];
    }

    /// <inheritdoc/>
    public bool Allows(ReadOnlySpan<char> text)
    {
        char[]? rented = null;
        Span<char> buffer = text.Length <= StackFoldLength
            ? stackalloc char[StackFoldLength]
            : (rented = ArrayPool<char>.Shared.Rent(text.Length));
        try
        {
            int start = -1;
            for (int i = 0; i <= text.Length; i++)
            {
                if (i < text.Length && (UserTextRules.IsLetter(text[i]) || UserTextRules.IsDigit(text[i])))
                {
                    if (start < 0)
                        start = i;
                    continue;
                }

                if (start >= 0 && Matches(text[start..i], buffer))
                    return false;
                start = -1;
            }

            return true;
        }
        finally
        {
            if (rented is not null)
                ArrayPool<char>.Shared.Return(rented);
        }
    }

    private static Regex Compile(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, s_matchTimeout);

    private bool Matches(ReadOnlySpan<char> word, Span<char> buffer)
    {
        Span<char> folded = buffer[..word.Length];
        word.ToLowerInvariant(folded);
        if (_wordsBySpan.Contains(folded))
            return true;

        foreach (Regex pattern in _patterns)
        {
            try
            {
                if (pattern.IsMatch(word))
                    return true;
            }
            catch (RegexMatchTimeoutException)
            {
                return true;
            }
        }

        return false;
    }
}
