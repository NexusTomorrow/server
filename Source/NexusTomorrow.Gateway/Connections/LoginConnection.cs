using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Limits;
using NexusTomorrow.Network;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Gateway.Connections;

/// <summary>
/// The login connection: greets bare, takes one <see cref="LoginRequest"/>, and answers inside the login envelope under the build key
/// with <see cref="LoginAccepted"/> and <see cref="RealmHandoff"/>, or <see cref="LoginDenied"/>. The client keeps the connection open
/// after the handoff.
/// </summary>
public sealed class LoginConnection : ClientConnection
{
    private const uint Build = 16042;
    private const uint ProtocolVersion = 0x097998A0;
    private const ushort LoginEnvelope = 0x0076;

    private readonly AccountOwner _accounts;
    private readonly IWriteBehind _persistence;
    private readonly LoginRateLimiter _limiter;
    private readonly GatewayOptions _gateway;
    private readonly RealmOptions _realm;
    private readonly uint _worldAddress;
    private bool _answered;

    /// <summary>
    /// Create the connection.
    /// </summary>
    /// <exception cref="FormatException">The world's public address is not an IP address.</exception>
    /// <exception cref="ArgumentException">The world's public address is an IP address but not an IPv4 one.</exception>
    public LoginConnection(
        MessageCatalog catalog,
        InboundDecoder decoder,
        AccountOwner accounts,
        IWriteBehind persistence,
        LoginRateLimiter limiter,
        IOptions<GatewayOptions> gateway,
        IOptions<RealmOptions> realm,
        TimeProvider time,
        GatewayMetrics metrics,
        ILogger<LoginConnection> logger)
        : base(LoginEnvelope, catalog, decoder, gateway, time, metrics, logger)
    {
        _accounts = accounts;
        _persistence = persistence;
        _limiter = limiter;
        _gateway = gateway.Value;
        _realm = realm.Value;
        _worldAddress = ParseWorldAddress(_gateway.WorldPublicAddress);
    }

    /// <inheritdoc/>
    protected override void OnStart() => SendBare(new ServerHello { Build = Build, ProtocolVersion = ProtocolVersion });

    /// <inheritdoc/>
    protected override async ValueTask<bool> HandleAsync(IWireMessage message, OpcodeRow row)
    {
        if (message is not LoginRequest request || _answered)
        {
            Disconnect("a message other than one login request on the login connection");
            return false;
        }

        _answered = true;
        if (!_limiter.TryAttempt(RemoteAddress) || !_persistence.IsAvailable || request.Build != Build)
        {
            Send(new LoginDenied { Result = LoginResults.General });
            return true;
        }

        var reply = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (!_accounts.Post(new RedeemGameToken(request.GameToken.ToGuid(), request.AccountName, reply)))
        {
            Disconnect("the server is stopping");
            return false;
        }

        switch (await reply.Task.WaitAsync(Lifetime))
        {
            case LoginGranted grant:
                MarkAuthenticated();
                Send(new LoginAccepted { DroppedForLag = 0 });
                Send(Handoff(grant));
                return true;
            case LoginRefused refused:
                Send(new LoginDenied { Result = refused.Result });
                return true;
            default:
                return false;
        }
    }

    // The address bytes are in network order, so the first octet is the high byte of the host-order integer the client reads.
    private static uint ParseWorldAddress(string address)
    {
        var parsed = IPAddress.Parse(address);
        if (parsed.AddressFamily != AddressFamily.InterNetwork)
            throw new ArgumentException($"The world's public address must be IPv4, not {address}.", nameof(address));

        return BinaryPrimitives.ReadUInt32BigEndian(parsed.GetAddressBytes());
    }

    private RealmHandoff Handoff(LoginGranted grant)
    {
        var handoff = new RealmHandoff
        {
            WorldAddress = _worldAddress,
            WorldPort = (ushort)_gateway.World.Port,
            Echo = grant.Echo,
            RealmName = _realm.Name,
            RealmFlags = _realm.Flags,
            PvpType = _realm.PvpType,
            NoteTextId = _realm.NoteTextId,
        };

        // SessionKey is get-only, so its fields are set one by one.
        var key = NetworkGuid.FromBytes(grant.SessionKey);
        handoff.SessionKey.Data1 = key.Data1;
        handoff.SessionKey.Data2 = key.Data2;
        handoff.SessionKey.Data3 = key.Data3;
        handoff.SessionKey.Data4 = key.Data4;
        return handoff;
    }
}
