using System.Text;

namespace NexusTomorrow.Commands;

/// <summary>
/// Splits a command line into tokens.
/// </summary>
public static class CommandLine
{
    /// <summary>
    /// Split <paramref name="line"/> on spaces outside double quotes; a quoted run joins its token, quotes are dropped, and a quoted
    /// empty string is an empty token. False, with no tokens, when a quote is left open.
    /// </summary>
    public static bool TrySplit(string line, out List<string> tokens)
    {
        tokens = [];
        var current = new StringBuilder();
        bool quoted = false;
        bool present = false;
        foreach (char c in line)
        {
            if (c == '"')
            {
                quoted = !quoted;
                present = true;
            }
            else if (c == ' ' && !quoted)
            {
                if (present)
                    tokens.Add(current.ToString());
                current.Clear();
                present = false;
            }
            else
            {
                current.Append(c);
                present = true;
            }
        }

        if (quoted)
        {
            tokens = [];
            return false;
        }

        if (present)
            tokens.Add(current.ToString());
        return true;
    }
}
