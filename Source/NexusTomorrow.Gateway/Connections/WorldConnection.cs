using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.World;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Connections;

/// <summary>
/// The world connection: greets inside the world envelope under the build key, redeems the session key through the account owner,
/// switches both directions to the session cipher, hands character-select messages and, from the world, chat lines to the realm
/// owner, and routes in-world messages to the map holding the session's character.
/// </summary>
public sealed class WorldConnection : ClientConnection, IWorldSession
{
    private const uint Build = 16042;
    private const uint ProtocolVersion = 0x097998A0;
    private const ushort WorldEnvelope = 0x03DC;

    private readonly AccountOwner _accounts;
    private readonly RealmOwner _realm;
    private readonly RealmOptions _realmOptions;
    private readonly ILogger _logger;
    private readonly SessionKeyRedeemer _redeem;
    private readonly Lock _route = new();
    private long? _accountId;
    private bool _greeted;

    // Guarded by _route: the map the session is routed to, whether the connection has ended, and whether the client's unit stands on a
    // platform (its positions then are not world coordinates). TryEnterMap refuses once it has ended, so a map is never kept past the
    // one OnClosed took, and clears the platform state, since each map starts in world coordinates.
    private IMapInbox? _map;
    private bool _ended;
    private bool _onPlatform;

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
    /// <remarks>
    /// Before the account owner's grant, only the greeting, and nothing while it is checked: anything else ends the connection unread.
    /// After it, a map row only while the session is routed to a map: one with no map is dropped unread and the session goes on.
    /// </remarks>
    protected override bool Admits(OpcodeRow row)
    {
        if (_accountId is null)
        {
            if (_greeted)
                return End("a message while the greeting is checked");

            return row.Opcode == OpcodeOf<WorldHello>() || End("a message before the greeting");
        }

        if (row.Owner == MessageOwner.Map && !InMap())
        {
            Drop(row.Opcode, "a map message with no map");
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    protected override async ValueTask<bool> HandleAsync(IWireMessage message, OpcodeRow row)
    {
        // Admits is the admission for the greeting and for a map row with no map. The greeting checks here and in GreetAsync are
        // guards (GreetAsync's holds the cast), and so is RouteToMap's map check, which also catches a map that handed the character
        // back between the admission and this handling.
        if (_accountId is not long accountId)
            return _greeted ? End("a message while the greeting is checked") : await GreetAsync(message);

        if (row.Owner == MessageOwner.Map)
            return RouteToMap(message, row);

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
                if (InMap())
                {
                    Drop(row.Opcode, "a select while in the world");
                    return true;
                }

                return Post(new SelectCharacter(accountId, this, select.CharacterId));
            case ChatSend say:
                // Reads only ChatReads.Members.
                if (!InMap())
                {
                    Drop(row.Opcode, "chat outside the world");
                    return true;
                }

                return Post(new ChatSaid(accountId, this, say.Channel.Type, say.Text, say.ChatId));
            case PregameKeepAlive:
                return true;
            default:
                return End($"an unexpected {message.GetType().Name} on the world connection");
        }
    }

    /// <inheritdoc/>
    protected override void OnClosed()
    {
        // A map taken here hears of the end once: the connection holds no map afterwards, so LeaveMap and a later TryEnterMap change
        // nothing.
        IMapInbox? map;
        lock (_route)
        {
            _ended = true;
            map = _map;
            _map = null;
        }

        map?.Post(new ClientLeft(this));

        // Both owners are told only when the account owner granted this session, which sets _accountId; a connection that ended before
        // the grant was read tells them from GreetAsync once the grant arrives.
        if (_accountId is long accountId)
            CloseSession(accountId);
    }

    /// <inheritdoc/>
    public bool TryEnterMap(IMapInbox map)
    {
        lock (_route)
        {
            if (_ended)
                return false;

            _map = map;
            _onPlatform = false;
            return true;
        }
    }

    /// <inheritdoc/>
    public void LeaveMap(IMapInbox map)
    {
        lock (_route)
        {
            if (ReferenceEquals(_map, map))
                _map = null;
        }
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

    private bool InMap()
    {
        lock (_route)
            return _map is not null;
    }

    // The map is read under _route and posted to outside it: a post never runs under the lock, and a map that has stopped refuses it,
    // which is not a drop. A movement is read under the lock, which guards the platform state it carries. One with an arm the client
    // never builds is dropped and the session goes on; one that gives no position, facing or relayed command is ignored, not dropped.
    // A null map here is the guard behind Admits: the map can hand the character back after the admission.
    private bool RouteToMap(IWireMessage message, OpcodeRow row)
    {
        IMapInbox? map;
        MovementRead read = default;
        lock (_route)
        {
            map = _map;
            if (map is not null && message is ClientMovement movement)
                read = MovementReads.Read(movement, ref _onPlatform);
        }

        if (map is null)
        {
            Drop(row.Opcode, "a map message with no map");
            return true;
        }

        if (message is ClientMovement)
        {
            if (read.Refusal == MovementRefusal.UnconfirmedArm)
            {
                Drop(row.Opcode, "an unconfirmed command arm");
                return true;
            }

            if (read.Forced != ForcedMoveArms.None)
                GatewayLog.ForcedMoveArrived(_logger, Id, read.Forced);
            if (!read.Routes)
                return true;
        }

        MapMessage? routed = message switch
        {
            ClientMovement => new ClientMoved(this, read.Sample, read.Batch),
            WorldReady ready => new ClientWorldReady(this, ready.ZoneId),
            MovementControlReceipt receipt => new ClientControlReceipt(this, receipt.Ticket),
            ZoneChange change => new ClientZoneChanged(this, change.ZoneId),
            LogoutRequest request => new ClientLogoutRequested(this, request.Begin),
            LogoutConfirm => new ClientLogoutConfirmed(this),
            _ => null,
        };
        if (routed is null)
        {
            Drop(row.Opcode, "a map message with no handler");
            return true;
        }

        _ = row.Priority == MessagePriority.High ? map.PostHigh(routed) : map.Post(routed);
        return true;
    }

    private Task<WorldDecision>? RedeemThroughOwner(WorldConnection connection, WorldHello hello, byte[] sessionKey)
    {
        var reply = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        return _accounts.Post(new RedeemSessionKey(sessionKey, hello.AccountId, hello.AccountName, connection, reply)) ? reply.Task : null;
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
