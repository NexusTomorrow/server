using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// The login server accepts the login.
/// </summary>
/// <remarks>
/// Taken only on the login connection while the client waits for an answer to <see cref="LoginRequest"/>. The client then waits for
/// <see cref="RealmHandoff"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0591, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class LoginAccepted
{
    /// <summary>
    /// Bit 0 set means the player's previous session was dropped for lag, which the login screen can show. The client ignores the other bits.
    /// </summary>
    [UInt(32)] public uint DroppedForLag { get; set; }
}
