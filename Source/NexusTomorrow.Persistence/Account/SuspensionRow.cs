namespace NexusTomorrow.Persistence.Account;

#pragma warning disable CS1591 // Data-shape code: the type's summary names the table; a column's name is its meaning (code standard, Documentation).

/// <summary>
/// <c>account.suspension</c>: a stretch of time an account may not log in; <see cref="EndsAt"/> null means until lifted.
/// </summary>
public sealed class SuspensionRow
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public DateTimeOffset StartsAt { get; set; }

    public DateTimeOffset? EndsAt { get; set; }

    public string Reason { get; set; } = "";

    public string CreatedBy { get; set; } = "";
}
