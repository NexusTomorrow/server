using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Gateway.Catalog;

/// <summary>
/// Why <see cref="MessageCatalog.CreateInbound(ushort, bool, out InboundRefusal)"/> refused an opcode.
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

    /// <summary>
    /// The message is declared but not read to <see cref="Rung.Purpose"/>, so the server may not read it even for its verified members.
    /// </summary>
    BelowPurpose,
}

/// <summary>
/// Every declared message with its rung, built once from the three message assemblies. The server reads only messages read to
/// <see cref="Rung.Fields"/> or above (or <see cref="Rung.Purpose"/> for a row that reads only verified members), and sends what
/// <see cref="MaySend"/> allows.
/// </summary>
public sealed class MessageCatalog
{
    // A delegate, because IWireMessage declares static abstract members and so cannot be a type argument of Func.
    internal delegate IWireMessage? MessageFactory(ushort opcode);

    private readonly FrozenDictionary<ushort, MessageFactory> _inbound;
    private readonly FrozenDictionary<ushort, Rung?> _inboundRungs;
    private readonly FrozenDictionary<Type, Rung?> _rungs;
    private readonly FrozenSet<Type> _dormant;

    private MessageCatalog(
        FrozenDictionary<ushort, MessageFactory> inbound, FrozenDictionary<ushort, Rung?> inboundRungs, FrozenDictionary<Type, Rung?> rungs, FrozenSet<Type> dormant)
    {
        _inbound = inbound;
        _inboundRungs = inboundRungs;
        _rungs = rungs;
        _dormant = dormant;
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
    public IWireMessage? CreateInbound(ushort opcode, out InboundRefusal refusal) => CreateInbound(opcode, admitPurpose: false, out refusal);

    /// <summary>
    /// A fresh instance of the client message <paramref name="opcode"/> names, or null with the reason when the server may not read it.
    /// With <paramref name="admitPurpose"/>, a message read to <see cref="Rung.Purpose"/> is admitted too, for a row whose handler reads
    /// only members marked verified.
    /// </summary>
    public IWireMessage? CreateInbound(ushort opcode, bool admitPurpose, out InboundRefusal refusal)
    {
        if (!_inboundRungs.TryGetValue(opcode, out Rung? rung))
        {
            refusal = InboundRefusal.Undeclared;
            return null;
        }

        Rung floor = admitPurpose ? Rung.Purpose : Rung.Fields;
        if (rung is null || rung < floor)
        {
            refusal = admitPurpose ? InboundRefusal.BelowPurpose : InboundRefusal.BelowFields;
            return null;
        }

        refusal = InboundRefusal.None;
        return _inbound[opcode](opcode);
    }

    /// <summary>
    /// True when the server may send <paramref name="message"/>: its type is not <see cref="DormantAttribute"/>, and it is read to
    /// <see cref="Rung.Fields"/> or above, or read to <see cref="Rung.Purpose"/> with every member set to anything but its default marked
    /// <see cref="VerifiedMemberAttribute"/>. Otherwise false, with <paramref name="refusal"/> naming the type and why: dormant whatever its
    /// rung, below <see cref="Rung.Purpose"/>, or at <see cref="Rung.Purpose"/> the first unmarked member set.
    /// </summary>
    public bool MaySend(IWireMessage message, [NotNullWhen(false)] out string? refusal)
    {
        Type type = message.GetType();
        if (_dormant.Contains(type))
        {
            refusal = $"{type.Name} is dormant";
            return false;
        }

        Rung? rung = _rungs.GetValueOrDefault(type);
        if (rung >= Rung.Fields)
        {
            refusal = null;
            return true;
        }

        if (rung != Rung.Purpose)
        {
            refusal = $"{type.Name} is not read to Purpose";
            return false;
        }

        if (message.FindUnverifiedValue() is { } path)
        {
            refusal = $"{type.Name}.{path} is set, but its meaning is not verified";
            return false;
        }

        refusal = null;
        return true;
    }

    /// <summary>
    /// The rung <paramref name="messageType"/> is verified to, or null when it is unverified or undeclared.
    /// </summary>
    public Rung? RungOf(Type messageType) => _rungs.GetValueOrDefault(messageType);

    // The catalog of the given assemblies' generated registries; Default is the three message assemblies.
    internal static MessageCatalog Build((ImmutableArray<WireLayout> Layouts, MessageFactory ClientToServer, MessageFactory ServerToClient)[] assemblies)
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

        var dormant = rungs.Keys.Where(type => type.IsDefined(typeof(DormantAttribute), inherit: false)).ToFrozenSet();
        return new MessageCatalog(inbound.ToFrozenDictionary(), inboundRungs.ToFrozenDictionary(), rungs.ToFrozenDictionary(), dormant);
    }
}
