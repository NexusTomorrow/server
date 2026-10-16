using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

/// <summary>
/// The server's greeting, the first message on every connection.
/// </summary>
/// <remarks>
/// On the login connection the client checks <see cref="Build"/> and <see cref="ProtocolVersion"/> and answers with <c>LoginRequest</c>
/// (0x0592). On a world connection it reads only <see cref="RealmId"/>, answers with <c>WorldHello</c> (0x058F) and then switches the
/// connection to the key derived from the session key. The client takes a bare greeting only on the login connection and drops one that
/// arrives bare on any other. The client registers a writer for it as well, and no client code sends it.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0003, Direction.Both)]
[Verified(Rung.Fields)]
public sealed partial class ServerHello
{
    /// <summary>
    /// The client's build. On the login connection the client ends the login unless this is 16042.
    /// </summary>
    [UInt(32)] public uint Build { get; set; }

    /// <summary>
    /// The realm the connection serves. The client reads it only on the world connection the login opens, and keeps it as its current realm.
    /// </summary>
    [UInt(32)] public uint RealmId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown3 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown4 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown5 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(5)] public byte Unknown6 { get; set; }

    /// <summary>
    /// The protocol version. On the login connection the client ends the login unless this is 0x097998A0.
    /// </summary>
    [UInt(32)] public uint ProtocolVersion { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown8 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown9 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown10 { get; set; }
}
