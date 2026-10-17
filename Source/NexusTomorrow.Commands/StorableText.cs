using System.Text;

namespace NexusTomorrow.Commands;

// Text that reaches a database column or a log must survive the round trip: PostgreSQL rejects a NUL, and a lone surrogate has no
// UTF-8 form. A write that fails for either reason is retried by the write-behind queue for good, so the dispatcher keeps such text
// out of every write (the audit row, and the commands it hands arguments to).
internal static class StorableText
{
    /// <summary>
    /// Whether <paramref name="value"/> holds a control character or an unpaired surrogate.
    /// </summary>
    public static bool IsUnstorable(string value) => !string.Equals(Scrub(value), value, StringComparison.Ordinal);

    /// <summary>
    /// <paramref name="value"/> with each control character and unpaired surrogate replaced by a question mark.
    /// </summary>
    public static string Scrub(string value)
    {
        StringBuilder? scrubbed = null;
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            bool pair = char.IsHighSurrogate(c) && i + 1 < value.Length && char.IsLowSurrogate(value[i + 1]);
            if (pair)
            {
                scrubbed?.Append(c).Append(value[i + 1]);
                i++;
            }
            else if (char.IsControl(c) || char.IsSurrogate(c))
            {
                scrubbed ??= new StringBuilder(value, 0, i, value.Length);
                scrubbed.Append('?');
            }
            else
            {
                scrubbed?.Append(c);
            }
        }

        return scrubbed?.ToString() ?? value;
    }
}
