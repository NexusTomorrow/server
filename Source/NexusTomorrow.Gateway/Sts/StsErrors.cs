namespace NexusTomorrow.Gateway.Sts;

/// <summary>
/// The STS error codes the server sends. Every refusal uses <see cref="InvalidAccountNameOrPassword"/>, the one error the client is known
/// to show, until other codes are read.
/// </summary>
public static class StsErrors
{
    /// <summary>
    /// The account name or password is wrong, and the code every refusal carries.
    /// </summary>
    public const int InvalidAccountNameOrPassword = 3002;
}
