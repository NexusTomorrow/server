using NexusTomorrow.Settings;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The <c>Account</c> configuration section.
/// </summary>
[SettingsSection(Section, 2, "Owners", "Owners#knobs")]
public sealed class AccountOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Account";

    /// <summary>
    /// How long a game token or a session key stays redeemable.
    /// </summary>
    /// <remarks>
    /// About a minute (decisions.md D9 2b).
    /// </remarks>
    [Positive]
    public TimeSpan TokenLifetime { get; set; } = TimeSpan.FromSeconds(60);
}
