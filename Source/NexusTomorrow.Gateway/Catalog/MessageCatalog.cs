using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Gateway.Catalog;

/// <summary>
/// Why <see cref="MessageCatalog.CreateInbound"/> refused an opcode.
/// </summary>
public enum InboundRefusal
{
    /// <summary>
    /// Not refused.
    /// </summary>
    None,

    /// <summary>
    /// No assembly declares a client message with this opcode.
    /// </summary>
    Undeclared,

    /// <summary>
    /// The message is declared but not read to <see cref="Rung.Fields"/>, so the server may not read it.
    /// </summary>
    BelowFields,
}

/// <summary>
/// Every declared message with its rung, built once from the three message assemblies. The server reads and sends only messages read
/// to <see cref="Rung.Fields"/> or above.
/// </summary>
public sealed class MessageCatalog
{
    // A delegate, because IWireMessage declares static abstract members and so cannot be a type argument of Func.
    private delegate IWireMessage? MessageFactory(ushort opcode);

    private readonly FrozenDictionary<ushort, MessageFactory> _inbound;
    private readonly FrozenDictionary<ushort, Rung?> _inboundRungs;
    private readonly FrozenDictionary<Type, Rung?> _rungs;

    private MessageCatalog(FrozenDictionary<ushort, MessageFactory> inbound, FrozenDictionary<ushort, Rung?> inboundRungs, FrozenDictionary<Type, Rung?> rungs)
    {
        _inbound = inbound;
        _inboundRungs = inboundRungs;
        _rungs = rungs;
    }

    /// <summary>
    /// The catalog of the Network, Network.Auth and Network.World assemblies.
    /// </summary>
    public static MessageCatalog Default { get; } = Build(
    [
        (global::NexusTomorrow.Network.WireMessages.Layouts, global::NexusTomorrow.Network.WireMessages.CreateClientToServer, global::NexusTomorrow.Network.WireMessages.CreateServerToClient),
        (global::NexusTomorrow.Network.Auth.WireMessages.Layouts, global::NexusTomorrow.Network.Auth.WireMessages.CreateClientToServer, global::NexusTomorrow.Network.Auth.WireMessages.CreateServerToClient),
        (global::NexusTomorrow.Network.World.WireMessages.Layouts, global::NexusTomorrow.Network.World.WireMessages.CreateClientToServer, global::NexusTomorrow.Network.World.WireMessages.CreateServerToClient),
    ]);

    /// <summary>
    /// A fresh instance of the client message <paramref name="opcode"/> names, or null with the reason when the server may not read it.
    /// </summary>
    public IWireMessage? CreateInbound(ushort opcode, out InboundRefusal refusal)
    {
        if (!_inboundRungs.TryGetValue(opcode, out Rung? rung))
        {
            refusal = InboundRefusal.Undeclared;
            return null;
        }

        if (rung is null or < Rung.Fields)
        {
            refusal = InboundRefusal.BelowFields;
            return null;
        }

        refusal = InboundRefusal.None;
        return _inbound[opcode](opcode);
    }

    /// <summary>
    /// True when <paramref name="messageType"/> is a declared server message read to <see cref="Rung.Fields"/> or above.
    /// </summary>
    public bool IsSendable(Type messageType) => _rungs.TryGetValue(messageType, out Rung? rung) && rung >= Rung.Fields;

    /// <summary>
    /// The rung <paramref name="messageType"/> is verified to, or null when it is unverified or undeclared.
    /// </summary>
    public Rung? RungOf(Type messageType) => _rungs.GetValueOrDefault(messageType);

    private static MessageCatalog Build((ImmutableArray<WireLayout> Layouts, MessageFactory ClientToServer, MessageFactory ServerToClient)[] assemblies)
    {
        var inbound = new Dictionary<ushort, MessageFactory>();
        var inboundRungs = new Dictionary<ushort, Rung?>();
        var rungs = new Dictionary<Type, Rung?>();
        foreach ((ImmutableArray<WireLayout> layouts, MessageFactory clientToServer, MessageFactory serverToClient) in assemblies)
        {
            foreach (WireLayout layout in layouts)
            {
                ushort opcode = layout.Opcode;
                if (layout.Direction.HasFlag(Direction.ClientToServer) && clientToServer(opcode) is { } client)
                {
                    Rung? rung = client.GetType().GetCustomAttribute<VerifiedAttribute>()?.Rung;
                    inbound.Add(opcode, clientToServer);
                    inboundRungs.Add(opcode, rung);
                    rungs[client.GetType()] = rung;
                }

                if (layout.Direction.HasFlag(Direction.ServerToClient) && serverToClient(opcode) is { } server)
                    rungs[server.GetType()] = server.GetType().GetCustomAttribute<VerifiedAttribute>()?.Rung;
            }
        }

        return new MessageCatalog(inbound.ToFrozenDictionary(), inboundRungs.ToFrozenDictionary(), rungs.ToFrozenDictionary());
    }
}
