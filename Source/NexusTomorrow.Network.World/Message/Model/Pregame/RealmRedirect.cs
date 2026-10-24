using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The server's answer to a <see cref="RealmChoice"/> of another realm: the session key and the realm's details.
/// </summary>
/// <remarks>
/// Taken on the realm-select connection.
/// </remarks>
[WireMessage(GameMessageOpcode.Server07A1, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class RealmRedirect
{
    [WireStruct]
    public sealed partial class Server07A1Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The session key, in the GUID layout. The client sends it back in <see cref="WorldHello"/> on the new connection and derives that
    /// connection's key from it.
    /// </summary>
    [WireSecret] public NetworkGuid SessionKey { get; } = new();

    public Server07A1Part0 Unknown2 { get; set; } = new();

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
