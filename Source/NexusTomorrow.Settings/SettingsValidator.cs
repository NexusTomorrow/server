using Microsoft.Extensions.Options;

namespace NexusTomorrow.Settings;

/// <summary>
/// Checks an options class against the rules its settings declare (<see cref="SettingsSection"/>) and reports every failure, each
/// naming its setting's key and never its value.
/// </summary>
public sealed class SettingsValidator<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly SettingsSection _section = SettingsSection.Of(typeof(TOptions));

    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        IReadOnlyList<string> failures = _section.Check(options);
        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
