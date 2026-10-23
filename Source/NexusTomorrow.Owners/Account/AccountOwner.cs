using System.Buffers.Binary;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The account side (decisions.md D9 2b): game tokens and session keys, each redeemable once within the token lifetime, with one live
/// game token per account; the world session of each account, where a second login replaces the first; and the admin changes to
/// accounts. Every suspension change goes through this owner, which keeps the state that overrides what the credentials said at
/// login, so a refusal here never disagrees with the database once the change has committed.
/// </summary>
public sealed class AccountOwner : Owner<AccountMessage>
{
    private const int SessionKeyBytes = 16;

    private readonly IWriteBehind _persistence;
    private readonly TimeProvider _time;
    private readonly TimeSpan _lifetime;
    private readonly Dictionary<Guid, PendingToken> _tokens = [];

    // The token in _tokens of each account that has one. A new token supersedes the account's earlier one, so _tokens holds at most
    // one entry per account however often a client asks, and every token in _tokens is its account's entry here.
    private readonly Dictionary<long, Guid> _tokenOf = [];
    private readonly Dictionary<UInt128, PendingSession> _sessionKeys = [];
    private readonly Dictionary<long, OnlineSession> _online = [];
    private readonly Dictionary<long, SuspensionOverride> _suspensions = [];
    private int _onlineCount;
    private int _pendingGameTokens;

    /// <summary>
    /// Create the account owner.
    /// </summary>
    public AccountOwner(TimeProvider time, IOptions<AccountOptions> options, ILogger<AccountOwner> logger, IWriteBehind persistence)
        : base("account", logger)
    {
        _persistence = persistence;
        _time = time;
        _lifetime = options.Value.TokenLifetime;
    }

    /// <summary>
    /// Accounts with a world session, read from any thread.
    /// </summary>
    public int OnlineCount => Volatile.Read(ref _onlineCount);

    // Game tokens held, read from any thread. Expired tokens stay counted until the next Issue, Redeem or session-key redeem sweeps them.
    internal int PendingGameTokens => Volatile.Read(ref _pendingGameTokens);

    /// <inheritdoc/>
    protected override void Handle(AccountMessage message)
    {
        switch (message)
        {
            case IssueGameToken issue:
                Guid issued = Issue(issue.Account);
                PublishPendingGameTokens();
                issue.Reply.TrySetResult(issued);
                break;
            case RedeemGameToken redeem:
                LoginDecision login = Redeem(redeem);
                PublishPendingGameTokens();
                redeem.Reply.TrySetResult(login);
                break;
            case RedeemSessionKey greet:
                WorldDecision world = Greet(greet);
                PublishPendingGameTokens();
                greet.Reply.TrySetResult(world);
                break;
            case WorldSessionClosed closed:
                if (_online.TryGetValue(closed.AccountId, out OnlineSession? current) && current.Session == closed.Session)
                    _online.Remove(closed.AccountId);
                Volatile.Write(ref _onlineCount, _online.Count);
                break;
            case FindOnlineSession find:
                find.Reply.TrySetResult(Find(find.Email));
                break;
            case CreateAccount create:
                _persistence.Enqueue(new CreateAccountWork(
                    create.Email, create.Salt, create.Verifier, _time.GetUtcNow(), result => Complete(new AccountCreated(result, create.Reply), create.Reply)));
                break;
            case AccountCreated created:
                created.Reply.TrySetResult(created.Result.Outcome == WorkOutcome.Committed
                    ? new AdminResult(true, "created")
                    : new AdminResult(false, "an account with that email exists"));
                break;
            case SuspendAccount suspend:
                _persistence.Enqueue(new SuspendAccountWork(
                    suspend.Email, _time.GetUtcNow(), suspend.Until, suspend.Reason, suspend.By,
                    id => Complete(new SuspensionStored(id, suspend.Until, Lifted: false, suspend.Reply), suspend.Reply)));
                break;
            case LiftSuspension lift:
                _persistence.Enqueue(new LiftSuspensionWork(
                    lift.Email, _time.GetUtcNow(), id => Complete(new SuspensionStored(id, Until: null, Lifted: true, lift.Reply), lift.Reply)));
                break;
            case SuspensionStored stored:
                Stored(stored);
                break;
        }
    }

    // Called before a reply that follows a change to _tokens completes, so a caller resumed by that reply reads the count it left.
    private void PublishPendingGameTokens() => Volatile.Write(ref _pendingGameTokens, _tokens.Count);

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
            case FindOnlineSession find:
                find.Reply.TrySetResult(null);
                break;
        }

        if (AdminReplyOf(message) is { } adminReply)
            adminReply.TrySetResult(new AdminResult(false, "the account owner failed"));
    }

    private static TaskCompletionSource<AdminResult>? AdminReplyOf(AccountMessage message) => message switch
    {
        CreateAccount m => m.Reply,
        SuspendAccount m => m.Reply,
        LiftSuspension m => m.Reply,
        AccountCreated m => m.Reply,
        SuspensionStored m => m.Reply,
        _ => null,
    };

    private static UInt128 KeyOf(byte[] sessionKey) => BinaryPrimitives.ReadUInt128LittleEndian(sessionKey);

    private Guid Issue(AccountCredentials account)
    {
        Expire();
        if (_tokenOf.Remove(account.Id, out Guid earlier))
            _tokens.Remove(earlier);

        var token = Guid.NewGuid();
        _tokens[token] = new PendingToken(account, _time.GetUtcNow() + _lifetime);
        _tokenOf[account.Id] = token;
        return token;
    }

    private LoginDecision Redeem(RedeemGameToken redeem)
    {
        Expire();
        if (!_tokens.Remove(redeem.Token, out PendingToken? pending))
            return new LoginRefused(LoginResults.General);

        _tokenOf.Remove(pending.Account.Id);
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

        // A suspension stored after the key was issued still applies: the key outlives the token step by up to the token lifetime.
        if (IsSuspended(pending.AccountId, null))
            return new WorldRefused("the account is suspended");

        if (_online.TryGetValue(pending.AccountId, out OnlineSession? previous) && previous.Session != greet.Session)
        {
            // Kicked.Reason 0 has no message of its own; the client shows its general one (Server036A's declaration).
            previous.Session.Send(new Kicked { Reason = 0 });
            previous.Session.Disconnect("replaced by a new login");
        }

        _online[pending.AccountId] = new OnlineSession(greet.Session, pending.Email);
        Volatile.Write(ref _onlineCount, _online.Count);
        return new WorldGranted(pending.AccountId);
    }

    // Runs on a persistence worker: hands the work's outcome back to the owner. A stopped owner takes no more messages, so the
    // operator is told instead of left waiting.
    private void Complete(AccountMessage message, TaskCompletionSource<AdminResult> reply)
    {
        if (!Post(message))
            reply.TrySetResult(new AdminResult(false, "the server is stopping"));
    }

    private void Stored(SuspensionStored stored)
    {
        if (stored.AccountId is not { } accountId)
        {
            stored.Reply.TrySetResult(new AdminResult(false, "no account has that email"));
            return;
        }

        _suspensions[accountId] = stored.Lifted ? new SuspensionOverride(null, false, true) : Combine(accountId, stored.Until);
        if (IsSuspended(accountId, null) && _online.Remove(accountId, out OnlineSession? online))
        {
            online.Session.Send(new Kicked { Reason = 0 });
            online.Session.Disconnect("the account was suspended");
            Volatile.Write(ref _onlineCount, _online.Count);
        }

        stored.Reply.TrySetResult(new AdminResult(true, stored.Lifted ? "suspension lifted" : "suspended"));
    }

    // A stored suspension adds to what is in force: the later end wins and an indefinite one wins over any end. Only a lift replaces the
    // credentials' view, because credentials issued before the lift still carry the suspension it ended.
    private SuspensionOverride Combine(long accountId, DateTimeOffset? until)
    {
        _suspensions.TryGetValue(accountId, out SuspensionOverride? previous);
        bool indefinite = until is null || previous?.Indefinite == true;
        DateTimeOffset? end = indefinite ? null : previous?.Until is { } earlier && earlier > until ? earlier : until;
        return new SuspensionOverride(end, indefinite, previous?.ReplacesCredentials ?? false);
    }

    private bool IsSuspended(long accountId, AccountCredentials? credentials)
    {
        DateTimeOffset now = _time.GetUtcNow();
        bool fromCredentials = credentials is not null && (credentials.SuspendedIndefinitely || credentials.SuspendedUntil > now);
        if (!_suspensions.TryGetValue(accountId, out SuspensionOverride? stored))
            return fromCredentials;

        return stored.Indefinite || stored.Until > now || (!stored.ReplacesCredentials && fromCredentials);
    }

    // Proportional to the accounts online: a lookup an operator or a debug tool asks for, never one a client message causes.
    private IClientSession? Find(string email)
    {
        foreach (OnlineSession online in _online.Values)
        {
            if (string.Equals(online.Email, email, StringComparison.OrdinalIgnoreCase))
                return online.Session;
        }

        return null;
    }

    private void Expire()
    {
        DateTimeOffset now = _time.GetUtcNow();
        foreach ((Guid token, PendingToken pending) in _tokens)
        {
            if (pending.ExpiresAt <= now)
            {
                _tokens.Remove(token);
                _tokenOf.Remove(pending.Account.Id);
            }
        }

        foreach ((UInt128 key, PendingSession pending) in _sessionKeys)
        {
            if (pending.ExpiresAt <= now)
                _sessionKeys.Remove(key);
        }
    }

    private sealed record SuspensionOverride(DateTimeOffset? Until, bool Indefinite, bool ReplacesCredentials);

    private sealed record PendingToken(AccountCredentials Account, DateTimeOffset ExpiresAt);

    private sealed record PendingSession(long AccountId, string Email, uint Echo, DateTimeOffset ExpiresAt);

    private sealed record OnlineSession(IClientSession Session, string Email);
}
