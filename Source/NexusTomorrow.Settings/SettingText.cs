using System.Globalization;

namespace NexusTomorrow.Settings;

// How the rules write numbers and durations: with a thousands separator from 1,000 up on the page (Prose), in plain digits in a
// failure (Digits) and in a default.
internal static class SettingText
{
    private static readonly string[] s_counts = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    public static string Prose(long value) => value.ToString("#,0", CultureInfo.InvariantCulture);

    public static string Digits(long value) => value.ToString(CultureInfo.InvariantCulture);

    public static string Code(string text) => $"`{text}`";

    // An integer rule reads any integer type the binder fills; a rule on a property of another type is a declaration error.
    public static long Integer(object? value) => value switch
    {
        sbyte or byte or short or ushort or int or uint or long => Convert.ToInt64(value, CultureInfo.InvariantCulture),
        _ => throw new InvalidOperationException($"An integer rule is declared on a setting of type {value?.GetType().Name ?? "null"}."),
    };

    public static TimeSpan Duration(object? value) =>
        value is TimeSpan duration ? duration : throw new InvalidOperationException($"A duration rule is declared on a setting of type {value?.GetType().Name ?? "null"}.");

    // A duration is written in the unit of its setting's default: whole seconds as s, anything finer as ms.
    public static string DurationUnit(Setting setting) => DurationUnit(Duration(setting.Default));

    public static string DurationUnit(TimeSpan reference) => reference.Ticks % TimeSpan.TicksPerSecond == 0 ? "s" : "ms";

    public static string Amount(TimeSpan value, string unit, bool separators)
    {
        decimal amount = unit == "s" ? (decimal)value.Ticks / TimeSpan.TicksPerSecond : (decimal)value.Ticks / TimeSpan.TicksPerMillisecond;
        return amount.ToString(separators ? "#,0.###" : "0.###", CultureInfo.InvariantCulture);
    }

    // The largest whole unit, one of them spelled out: "one second", "5 seconds", "one minute".
    public static string Words(TimeSpan value)
    {
        (long count, string unit) = value.Ticks switch
        {
            not 0 and var ticks when ticks % TimeSpan.TicksPerMinute == 0 => (ticks / TimeSpan.TicksPerMinute, "minute"),
            var ticks when ticks % TimeSpan.TicksPerSecond == 0 => (ticks / TimeSpan.TicksPerSecond, "second"),
            var ticks => (ticks / TimeSpan.TicksPerMillisecond, "millisecond"),
        };
        return count == 1 ? $"one {unit}" : $"{Digits(count)} {unit}s";
    }

    // How many settings a rule names, spelled out below ten: "three different ports".
    public static string Count(int count) => count >= 0 && count < s_counts.Length ? s_counts[count] : Digits(count);

    // "a, b and c"
    public static string List(IEnumerable<string> items) => Join(items, "and");

    // "a, b or c"
    public static string Alternatives(IEnumerable<string> items) => Join(items, "or");

    private static string Join(IEnumerable<string> items, string last)
    {
        string[] all = [.. items];
        return all.Length < 2 ? string.Concat(all) : $"{string.Join(", ", all[..^1])} {last} {all[^1]}";
    }
}
