using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Connections;

/// <summary>
/// The world connection: greets inside the world envelope under the build key, redeems the session key through the account owner,
/// switches both directions to the session cipher, and hands character-select messages to the realm owner.
/// </summary>
public sealed class WorldConnection : ClientConnection
{
    private const uint Build = 16042;
    private const uint ProtocolVersion = 0x097998A0;
    private const ushort WorldEnvelope = 0x03DC;

    private readonly AccountOwner _accounts;
    private readonly RealmOwner _realm;
    private readonly RealmOptions _realmOptions;
    private readonly ILogger _logger;
    private readonly SessionKeyRedeemer _redeem;
    private long? _accountId;
    private bool _greeted;

    /// <summary>
    /// Create the connection.
    /// </summary>
    public WorldConnection(
        MessageCatalog catalog,
        InboundDecoder decoder,
        AccountOwner accounts,
        RealmOwner realm,
        IOptions<GatewayOptions> gateway,
        IOptions<RealmOptions> realmOptions,
        TimeProvider time,
        GatewayMetrics metrics,
        ILogger<WorldConnection> logger)
        : this(catalog, decoder, accounts, realm, gateway, realmOptions, time, metrics, logger, null)
    {
    }

    // The redeemer is the seam that lets a test hold or fault the account owner's decision; production passes null and redeems
    // through the account owner.
    internal WorldConnection(
        MessageCatalog catalog,
        InboundDecoder decoder,
        AccountOwner accounts,
        RealmOwner realm,
        IOptions<GatewayOptions> gateway,
        IOptions<RealmOptions> realmOptions,
        TimeProvider time,
        GatewayMetrics metrics,
        ILogger<WorldConnection> logger,
        SessionKeyRedeemer? redeemer)
        : base(WorldEnvelope, catalog, decoder, gateway, time, metrics, logger)
    {
        _accounts = accounts;
        _realm = realm;
        _realmOptions = realmOptions.Value;
        _logger = logger;
        _redeem = redeemer ?? RedeemThroughOwner;
    }

    // Starts the redemption and returns its answer, or null when the account owner no longer takes messages.
    internal delegate Task<WorldDecision>? SessionKeyRedeemer(WorldConnection connection, WorldHello hello, byte[] sessionKey);

    /// <inheritdoc/>
    protected override void OnStart() =>
        Send(new ServerHello { Build = Build, RealmId = _realmOptions.Id, ProtocolVersion = ProtocolVersion });

    /// <inheritdoc/>
    protected override async ValueTask<bool> HandleAsync(IWireMessage message)
    {
        if (_accountId is not long accountId)
            return _greeted ? End("a message while the greeting is checked") : await GreetAsync(message);

        switch (message)
        {
            case CharacterListRequest:
                return Post(new ListCharacters(accountId, this));
            case CharacterCreate create:
                return Post(new CreateCharacter(accountId, this, create));
            case CharacterDelete delete:
                return Post(new DeleteCharacter(accountId, this, delete.CharacterId));
            case CharacterRename rename:
                return Post(new RenameCharacter(accountId, this, rename.CharacterId, rename.NewName));
            case CharacterSelect select:
                return Post(new SelectCharacter(accountId, this, select.CharacterId));
            case PregameKeepAlive:
                return true;
            default:
                return End($"an unexpected {message.GetType().Name} on the world connection");
        }
    }

    /// <inheritdoc/>
    protected override void OnClosed()
    {
        // Both owners are told only when the account owner granted this session, which sets _accountId; a connection that ended before
        // the grant was read tells them from GreetAsync once the grant arrives.
        if (_accountId is long accountId)
            CloseSession(accountId);
    }

    private async ValueTask<bool> GreetAsync(IWireMessage message)
    {
        if (message is not WorldHello hello)
            return End("a message before the greeting");

        _greeted = true;
        byte[] sessionKey = new byte[16];
        hello.SessionKey.CopyTo(sessionKey);

        // The client switched to this key right after sending the greeting; the next frame is already under it.
        SwitchKey(new PacketCipher(PacketCipher.SessionKey(sessionKey)));

        Task<WorldDecision>? reply = _redeem(this, hello, sessionKey);
        if (reply is null)
            return End("the server is stopping");

        WorldDecision decision;
        try
        {
            decision = await reply.WaitAsync(Lifetime);
        }
        catch (OperationCanceledException) when (Lifetime.IsCancellationRequested)
        {
            // The connection ended while the account owner was deciding. A grant that lands afterwards put this session online, and
            // nothing else will take it off.
            _ = reply.ContinueWith(
                done =>
                {
                    if (done.Result is WorldGranted granted)
                        CloseSession(granted.AccountId);
                },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
            throw;
        }
        catch (Exception exception)
        {
            GatewayLog.ConnectionFailed(_logger, Id, exception);
            return End("the account owner failed");
        }

        switch (decision)
        {
            case WorldGranted granted:
                _accountId = granted.AccountId;
                MarkAuthenticated();
                return true;
            case WorldRefused refused:
                return End(refused.Reason);
            default:
                return End("an unrecognized decision from the account owner");
        }
    }

    private Task<WorldDecision>? RedeemThroughOwner(WorldConnection connection, WorldHello hello, byte[] sessionKey)
    {
        var reply = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        return _accounts.Post(new RedeemSessionKey(sessionKey, hello.Echo, hello.AccountName, connection, reply)) ? reply.Task : null;
    }

    private void CloseSession(long accountId)
    {
        _accounts.Post(new WorldSessionClosed(accountId, this));
        _realm.Post(new RealmSessionClosed(accountId, this));
    }

    private bool Post(RealmMessage message) => _realm.Post(message) || End("the server is stopping");

    private bool End(string reason)
    {
        Disconnect(reason);
        return false;
    }
}
