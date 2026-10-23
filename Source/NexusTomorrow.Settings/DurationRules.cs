using System.Globalization;

namespace NexusTomorrow.Settings;

/// <summary>
/// A duration setting that must be longer than zero.
/// </summary>
public sealed class PositiveAttribute : SettingRuleAttribute
{
    /// <inheritdoc/>
    public override string MustBe(Setting setting) => "Positive";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        SettingText.Duration(value) > TimeSpan.Zero ? null : $"{setting.Key} must be positive.";
}

/// <summary>
/// A duration setting's shortest value. The page writes it in the unit of the setting's default; the failure in words, such as
/// "one second" or "5 seconds".
/// </summary>
public sealed class AtLeastDurationAttribute : SettingRuleAttribute
{
    private readonly TimeSpan _minimum;

    /// <summary>
    /// Require at least <paramref name="minimum"/>, written as <see cref="TimeSpan"/> text (<c>00:00:01</c>).
    /// </summary>
    public AtLeastDurationAttribute(string minimum)
    {
        Minimum = minimum;
        _minimum = TimeSpan.Parse(minimum, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// The shortest duration allowed, as written.
    /// </summary>
    public string Minimum { get; }

    /// <inheritdoc/>
    public override string MustBe(Setting setting)
    {
        string unit = SettingText.DurationUnit(setting);
        return $"At least {SettingText.Amount(_minimum, unit, separators: true)} {unit}";
    }

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        SettingText.Duration(value) >= _minimum ? null : $"{setting.Key} must be at least {SettingText.Words(_minimum)}.";
}

/// <summary>
/// A duration setting's shortest and longest values, both allowed, written in the unit of the setting's default.
/// </summary>
public sealed class BetweenDurationsAttribute : SettingRuleAttribute
{
    private readonly TimeSpan _minimum;
    private readonly TimeSpan _maximum;

    /// <summary>
    /// Require <paramref name="minimum"/> to <paramref name="maximum"/>, each written as <see cref="TimeSpan"/> text (<c>00:01:00</c>).
    /// </summary>
    public BetweenDurationsAttribute(string minimum, string maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        _minimum = TimeSpan.Parse(minimum, CultureInfo.InvariantCulture);
        _maximum = TimeSpan.Parse(maximum, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// The shortest duration allowed, as written.
    /// </summary>
    public string Minimum { get; }

    /// <summary>
    /// The longest duration allowed, as written.
    /// </summary>
    public string Maximum { get; }

    /// <inheritdoc/>
    public override string MustBe(Setting setting) => Range(setting, separators: true);

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value)
    {
        TimeSpan duration = SettingText.Duration(value);
        return duration >= _minimum && duration <= _maximum ? null : $"{setting.Key} must be {Range(setting, separators: false)}.";
    }

    private string Range(Setting setting, bool separators)
    {
        string unit = SettingText.DurationUnit(setting);
        return $"{SettingText.Amount(_minimum, unit, separators)} to {SettingText.Amount(_maximum, unit, separators)} {unit}";
    }
}
