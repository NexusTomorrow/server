using System.Buffers.Binary;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Persistence.Account;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The account side (decisions.md D9 2b): game tokens and session keys, each redeemable once within the token lifetime; the world
/// session of each account, where a second login replaces the first; and suspensions changed while the server runs, which override
/// what the credentials said at login.
/// </summary>
public sealed class AccountOwner : Owner<AccountMessage>
{
    private const int SessionKeyBytes = 16;

    private readonly TimeProvider _time;
    private readonly TimeSpan _lifetime;
    private readonly Dictionary<Guid, PendingToken> _tokens = [];
    private readonly Dictionary<UInt128, PendingSession> _sessionKeys = [];
    private readonly Dictionary<long, IClientSession> _online = [];
    private readonly Dictionary<long, (DateTimeOffset? Until, bool Indefinite)> _suspensions = [];
    private int _onlineCount;

    /// <summary>
    /// Create the account owner.
    /// </summary>
    public AccountOwner(TimeProvider time, IOptions<AccountOptions> options, ILogger<AccountOwner> logger)
        : base("account", logger)
    {
        _time = time;
        _lifetime = options.Value.TokenLifetime;
    }

    /// <summary>
    /// Accounts with a world session, read from any thread.
    /// </summary>
    public int OnlineCount => Volatile.Read(ref _onlineCount);

    /// <inheritdoc/>
    protected override void Handle(AccountMessage message)
    {
        switch (message)
        {
            case IssueGameToken issue:
                Expire();
                var token = Guid.NewGuid();
                _tokens[token] = new PendingToken(issue.Account, _time.GetUtcNow() + _lifetime);
                issue.Reply.TrySetResult(token);
                break;
            case RedeemGameToken redeem:
                redeem.Reply.TrySetResult(Redeem(redeem));
                break;
            case RedeemSessionKey greet:
                greet.Reply.TrySetResult(Greet(greet));
                break;
            case WorldSessionClosed closed:
                if (_online.TryGetValue(closed.AccountId, out IClientSession? current) && current == closed.Session)
                    _online.Remove(closed.AccountId);
                Volatile.Write(ref _onlineCount, _online.Count);
                break;
            case SuspensionChanged changed:
                _suspensions[changed.AccountId] = (changed.Until, changed.Indefinite);
                if (IsSuspended(changed.AccountId, null) && _online.Remove(changed.AccountId, out IClientSession? online))
                {
                    online.Send(new Kicked { Reason = 0 });
                    online.Disconnect("the account was suspended");
                    Volatile.Write(ref _onlineCount, _online.Count);
                }

                break;
        }
    }

    /// <inheritdoc/>
    protected override void OnFault(AccountMessage message, Exception exception)
    {
        switch (message)
        {
            case IssueGameToken issue:
                issue.Reply.TrySetException(exception);
                break;
            case RedeemGameToken redeem:
                redeem.Reply.TrySetResult(new LoginRefused(LoginResults.General));
                break;
            case RedeemSessionKey greet:
                greet.Reply.TrySetResult(new WorldRefused("the account owner failed"));
                break;
        }
    }

    private static UInt128 KeyOf(byte[] sessionKey) => BinaryPrimitives.ReadUInt128LittleEndian(sessionKey);

    private LoginDecision Redeem(RedeemGameToken redeem)
    {
        Expire();
        if (!_tokens.Remove(redeem.Token, out PendingToken? pending))
            return new LoginRefused(LoginResults.General);
        if (!string.Equals(pending.Account.Email, redeem.AccountName, StringComparison.OrdinalIgnoreCase))
            return new LoginRefused(LoginResults.General);
        if (IsSuspended(pending.Account.Id, pending.Account))
            return new LoginRefused(LoginResults.General);

        byte[] sessionKey = RandomNumberGenerator.GetBytes(SessionKeyBytes);
        uint echo = BinaryPrimitives.ReadUInt32LittleEndian(RandomNumberGenerator.GetBytes(sizeof(uint)));
        _sessionKeys[KeyOf(sessionKey)] = new PendingSession(pending.Account.Id, pending.Account.Email, echo, _time.GetUtcNow() + _lifetime);
        return new LoginGranted(pending.Account.Id, sessionKey, echo);
    }

    private WorldDecision Greet(RedeemSessionKey greet)
    {
        Expire();
        if (greet.SessionKey.Length != SessionKeyBytes || !_sessionKeys.Remove(KeyOf(greet.SessionKey), out PendingSession? pending))
            return new WorldRefused("an unknown or expired session key");
        if (pending.Echo != greet.Echo || !string.Equals(pending.Email, greet.AccountName, StringComparison.OrdinalIgnoreCase))
            return new WorldRefused("a session key presented with the wrong echo or account name");

        if (_online.TryGetValue(pending.AccountId, out IClientSession? previous) && previous != greet.Session)
        {
            // Kicked.Reason 0 has no message of its own; the client shows its general one (Server036A's declaration).
            previous.Send(new Kicked { Reason = 0 });
            previous.Disconnect("replaced by a new login");
        }

        _online[pending.AccountId] = greet.Session;
        Volatile.Write(ref _onlineCount, _online.Count);
        return new WorldGranted(pending.AccountId);
    }

    private bool IsSuspended(long accountId, AccountCredentials? credentials)
    {
        (DateTimeOffset? until, bool indefinite) = _suspensions.TryGetValue(accountId, out (DateTimeOffset? Until, bool Indefinite) changed)
            ? changed
            : (credentials?.SuspendedUntil, credentials?.SuspendedIndefinitely ?? false);
        return indefinite || until > _time.GetUtcNow();
    }

    private void Expire()
    {
        DateTimeOffset now = _time.GetUtcNow();
        foreach ((Guid token, PendingToken pending) in _tokens)
        {
            if (pending.ExpiresAt <= now)
                _tokens.Remove(token);
        }

        foreach ((UInt128 key, PendingSession pending) in _sessionKeys)
        {
            if (pending.ExpiresAt <= now)
                _sessionKeys.Remove(key);
        }
    }

    private sealed record PendingToken(AccountCredentials Account, DateTimeOffset ExpiresAt);

    private sealed record PendingSession(long AccountId, string Email, uint Echo, DateTimeOffset ExpiresAt);
}
