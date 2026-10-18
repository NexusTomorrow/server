using System.Globalization;
using System.Security.Cryptography;
using NexusTomorrow.Cryptography;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;

namespace NexusTomorrow.Commands;

/// <summary>
/// The account commands. Each posts to the account owner and answers when the owner does, which is after the database commit.
/// </summary>
public static class AccountCommands
{
    private const int SaltBytes = 16;
    private const int MaxEmailLength = 254;
    private const int MaxPasswordLength = 128;

    // The width of account.suspension.reason (AccountContext).
    private const int MaxReasonLength = 200;

    // Past this many hours, DateTimeOffset.AddHours throws.
    private const double MaxSuspensionHours = 24 * 365 * 100;

    /// <summary>
    /// The account commands, each posting to <paramref name="accounts"/>; a timed suspension ends <paramref name="time"/>'s now plus
    /// its hours.
    /// </summary>
    public static IEnumerable<CommandDefinition> Create(AccountOwner accounts, TimeProvider time)
    {
        yield return new CommandDefinition
        {
            Name = "account create",
            Description = "Create an account that can log in.",
            Permission = PermissionKeys.AccountCreate,
            Parameters =
            [
                new("email", "the login name", IsValid: IsEmail, Invalid: "That is not an email address."),
                new("password", "the password", Secret: true),
            ],
            ExecuteAsync = async (invocation, cancellationToken) =>
            {
                // The dispatcher has checked the email, so a password typed in its place is refused unrecorded.
                string email = invocation.Arguments[0];
                string password = invocation.Arguments[1];
                if (password.Length is 0 or > MaxPasswordLength)
                    return new CommandReply(CommandOutcome.Refused, $"A password is 1 to {MaxPasswordLength} characters.");

                // The verifier's identity is the lowercased email, as PasswordProvider and the client's proof use it.
                byte[] salt = RandomNumberGenerator.GetBytes(SaltBytes);
                byte[] verifier = Srp6Provider.GenerateVerifier(salt, email.ToLowerInvariant(), password);
                return Reply(await AskAsync(accounts, reply => new CreateAccount(email, salt, verifier, reply), cancellationToken));
            },
        };
        yield return new CommandDefinition
        {
            Name = "account suspend",
            Description = "Suspend an account for a number of hours, or forever, and remove it from the world.",
            Permission = PermissionKeys.AccountSuspend,
            ConsoleOnly = true,
            Parameters =
            [
                new("email", "the login name"),
                new("hours", "a number of hours, or forever"),
                new("reason", "why, stored with the suspension", Optional: true, Rest: true),
            ],
            ExecuteAsync = async (invocation, cancellationToken) =>
            {
                string email = invocation.Arguments[0];
                string reason = invocation.Arguments.Count > 2 ? invocation.Arguments[2] : "";
                DateTimeOffset? until = null;
                if (!string.Equals(invocation.Arguments[1], "forever", StringComparison.OrdinalIgnoreCase))
                {
                    if (!double.TryParse(invocation.Arguments[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double hours)
                        || !(hours > 0 && hours <= MaxSuspensionHours))
                    {
                        return new CommandReply(CommandOutcome.Refused, "Hours is a positive number, or forever.");
                    }

                    until = time.GetUtcNow().AddHours(hours);
                }

                if (reason.Length > MaxReasonLength)
                    return new CommandReply(CommandOutcome.Refused, $"A reason is at most {MaxReasonLength} characters.");

                return Reply(await AskAsync(accounts, reply => new SuspendAccount(email, until, reason, invocation.Actor.Name, reply), cancellationToken));
            },
        };
        yield return new CommandDefinition
        {
            Name = "account unsuspend",
            Description = "End every suspension in force for an account.",
            Permission = PermissionKeys.AccountUnsuspend,
            ConsoleOnly = true,
            Parameters = [new("email", "the login name")],
            ExecuteAsync = async (invocation, cancellationToken) =>
                Reply(await AskAsync(accounts, reply => new LiftSuspension(invocation.Arguments[0], reply), cancellationToken)),
        };
    }

    private static bool IsEmail(string value) =>
        value.Length <= MaxEmailLength && value.IndexOf('@', StringComparison.Ordinal) > 0 && !value.Contains(' ', StringComparison.Ordinal);

    private static async Task<AdminResult> AskAsync(AccountOwner accounts, Func<TaskCompletionSource<AdminResult>, AccountMessage> message, CancellationToken cancellationToken)
    {
        var reply = new TaskCompletionSource<AdminResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (!accounts.Post(message(reply)))
            return new AdminResult(false, "The server is stopping.");
        return await reply.Task.WaitAsync(cancellationToken);
    }

    private static CommandReply Reply(AdminResult result) =>
        new(result.Succeeded ? CommandOutcome.Done : CommandOutcome.Refused, result.Text);
}
