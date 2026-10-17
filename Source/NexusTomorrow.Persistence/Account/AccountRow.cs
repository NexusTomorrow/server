namespace NexusTomorrow.Persistence.Account;

#pragma warning disable CS1591 // Data-shape code: the type's summary names the table; a column's name is its meaning (code standard, Documentation).

/// <summary>
/// <c>account.account</c>: one login, with the SRP6 salt and verifier its password makes.
/// </summary>
public sealed class AccountRow
{
    public long Id { get; set; }

    public string Email { get; set; } = "";

    public byte[] Salt { get; set; } = [];

    public byte[] Verifier { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
}
