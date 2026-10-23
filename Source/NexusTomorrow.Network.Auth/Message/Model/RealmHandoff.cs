using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// The login server hands the client to a realm's world server: where to connect, the session key, and the realm's details.
/// </summary>
/// <remarks>
/// Taken only on the login connection after <see cref="LoginAccepted"/>. The client opens a connection to the world server, starts it
/// under the build's fixed key, and waits there for <see cref="Network.Message.Model.ServerHello"/>; the login connection stays open.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03DB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RealmHandoff
{
    /// <summary>
    /// The world server's IPv4 address as a host-order integer: a.b.c.d is (a &lt;&lt; 24) | (b &lt;&lt; 16) | (c &lt;&lt; 8) | d.
    /// </summary>
    [UInt(32)] public uint WorldAddress { get; set; }

    /// <summary>
    /// The world server's TCP port.
    /// </summary>
    [UInt(16)] public ushort WorldPort { get; set; }

    /// <summary>
    /// The session key, in the GUID layout. The client sends it back in <c>WorldHello</c> (0x058F) and derives the world connection's key
    /// from it.
    /// </summary>
    [WireSecret] public NetworkGuid SessionKey { get; } = new();

    /// <summary>
    /// A value the client keeps and sends back unchanged as the first field of <c>WorldHello</c> (0x058F).
    /// </summary>
    [UInt(32)] public uint Echo { get; set; }

    /// <summary>
    /// The realm's name, shown to the player and used to name the client's settings file for the realm.
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
