using NexusTomorrow.Settings;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// A list of <see cref="EntitlementGrant"/>s: each entitlement listed once, by a nonzero id, with a count of at least 1 when one is
/// given. A rule on the list, since a rule inside a list is never checked (<see cref="SettingsSection"/>).
/// </summary>
public sealed class EntitlementGrantsAttribute : SettingRuleAttribute
{
    /// <inheritdoc/>
    public override string MustBe(Setting setting) => "Each entitlement once, by a nonzero id, with a count of at least 1 when one is given";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value)
    {
        if (value is not IEnumerable<EntitlementGrant> grants)
            return null;

        var seen = new HashSet<uint>();
        foreach (EntitlementGrant grant in grants)
        {
            if (grant.Id == 0 || !seen.Add(grant.Id) || grant.Count == 0)
                return $"{setting.Key} must list each entitlement once, by a nonzero id, with a count of at least 1 when one is given.";
        }

        return null;
    }
}
