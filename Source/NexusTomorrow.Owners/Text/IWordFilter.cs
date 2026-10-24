namespace NexusTomorrow.Owners.Text;

/// <summary>
/// The client's word filter at its Strict class. The client never runs it on a character name or a chat line, so the server is the
/// only filter either meets.
/// </summary>
public interface IWordFilter
{
    /// <summary>
    /// True when no word of <paramref name="text"/> matches a filter row.
    /// </summary>
    bool Allows(ReadOnlySpan<char> text);
}
