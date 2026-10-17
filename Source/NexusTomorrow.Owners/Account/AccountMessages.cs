using NexusTomorrow.Persistence.Account;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// A message the account owner handles.
/// </summary>
public abstract record AccountMessage;

/// <summary>
/// The STS proved the password: issue a game token for the account, answered with the token.
/// </summary>
public sealed record IssueGameToken(AccountCredentials Account, TaskCompletionSource<Guid> Reply) : AccountMessage;

/// <summary>
/// A client presented a game token: answered with the session key and echo for the realm handoff, or a refusal.
/// </summary>
public sealed record RedeemGameToken(Guid Token, string AccountName, TaskCompletionSource<LoginDecision> Reply) : AccountMessage;

/// <summary>
/// A client greeted the world with a session key: answered with the account it enters as, or a refusal.
/// </summary>
public sealed record RedeemSessionKey(
    byte[] SessionKey,
    uint Echo,
    string AccountName,
    IClientSession Session,
    TaskCompletionSource<WorldDecision> Reply) : AccountMessage;

/// <summary>
/// A world session ended. The account stays online if a newer session has replaced this one.
/// </summary>
public sealed record WorldSessionClosed(long AccountId, IClientSession Session) : AccountMessage;

/// <summary>
/// An operator changed an account's suspension while the server runs. A null <paramref name="Until"/> with
/// <paramref name="Indefinite"/> false lifts it.
/// </summary>
public sealed record SuspensionChanged(long AccountId, DateTimeOffset? Until, bool Indefinite) : AccountMessage;

/// <summary>
/// The answer to <see cref="RedeemGameToken"/>.
/// </summary>
public abstract record LoginDecision;

/// <summary>
/// The login may go on to the realm: the session key and echo the client carries to it.
/// </summary>
public sealed record LoginGranted(long AccountId, byte[] SessionKey, uint Echo) : LoginDecision;

/// <summary>
/// The login is refused with this <c>LoginDenied.Result</c> value.
/// </summary>
public sealed record LoginRefused(uint Result) : LoginDecision;

/// <summary>
/// The answer to <see cref="RedeemSessionKey"/>.
/// </summary>
public abstract record WorldDecision;

/// <summary>
/// The session may enter the world as this account.
/// </summary>
public sealed record WorldGranted(long AccountId) : WorldDecision;

/// <summary>
/// The session is refused, for the reason given (logged, never sent to the client).
/// </summary>
public sealed record WorldRefused(string Reason) : WorldDecision;
