namespace NexusTomorrow.Owners.Account;

/// <summary>
/// Values for <c>LoginDenied.Result</c> (0x063D) whose meaning its declaration documents.
/// </summary>
public static class LoginResults
{
    /// <summary>
    /// A value the client answers with its general message (14, 15 and anything above 27 do). Every refusal sends it until the
    /// declaration documents which numbered message means what.
    /// </summary>
    public const uint General = 15;
}
