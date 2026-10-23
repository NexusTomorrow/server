using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The server's answer to a <see cref="RealmChoice"/> of another realm: its world server, the session key and the realm's details.
/// </summary>
/// <remarks>
/// Taken on the realm-select connection. The client connects to <see cref="Gateway"/> and waits there for
/// <see cref="Network.Message.Model.ServerHello"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server07A1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RealmRedirect
{
    /// <summary>
    /// Where the realm's world server listens.
    /// </summary>
    [WireStruct]
    public sealed partial class RealmGateway
    {
        /// <summary>
        /// The IPv4 address as a host-order integer: a.b.c.d is (a &lt;&lt; 24) | (b &lt;&lt; 16) | (c &lt;&lt; 8) | d.
        /// </summary>
        [UInt(32)] public uint Address { get; set; }

        /// <summary>
        /// The TCP port.
        /// </summary>
        [UInt(16)] public ushort Port { get; set; }
    }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The session key, in the GUID layout. The client sends it back in <see cref="WorldHello"/> on the new connection and derives that
    /// connection's key from it.
    /// </summary>
    [Secret] public NetworkGuid SessionKey { get; } = new();

    /// <summary>
    /// The realm's world server.
    /// </summary>
    public RealmGateway Gateway { get; set; } = new();

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public bool Unknown3 { get; set; }

    /// <summary>
    /// The realm's name; the client keeps it and names its settings file for the realm with it.
    /// </summary>
    [WideString] public string RealmName { get; set; } = "";

    /// <summary>
    /// The realm's flags. Bit 4 set means the realm keeps characters to one faction; the client tests no other bit.
    /// </summary>
    [UInt(32)] public uint RealmFlags { get; set; }

    /// <summary>
    /// The realm's PvP type: 0 PvE, 1 PvP. The interface treats any other value as PvE.
    /// </summary>
    [UInt(2)] public byte PvpType { get; set; }

    /// <summary>
    /// A note about the realm, as an id in the client's text table; 0 means none.
    /// </summary>
    [UInt(21)] public uint NoteTextId { get; set; }
}
