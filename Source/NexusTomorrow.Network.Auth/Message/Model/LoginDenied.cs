using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// The login server refuses the login.
/// </summary>
/// <remarks>
/// Taken on the login connection in any state. The client shows a message chosen by <see cref="Result"/>, closes its connections and returns
/// to the login screen.
/// </remarks>
[WireMessage(GameMessageOpcode.Server063D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class LoginDenied
{
    /// <summary>
    /// Why the login was refused. The client has a message of its own for 0 to 13, 16 to 20 and 22 to 27, and a general message for 14, 15
    /// and anything above 27; for 21 it builds its message from <see cref="Duration"/>.
    /// </summary>
    [UInt(32)] public uint Result { get; set; }

    /// <summary>
    /// A number the client puts into the message it shows, for every result but 21.
    /// </summary>
    [UInt(32)] public uint ResultArgument { get; set; }

    /// <summary>
    /// Read only when <see cref="Result"/> is 21: the client multiplies it by 24 and shows the product, to one decimal place, in that
    /// result's message.
    /// </summary>
    [Float32] public float Duration { get; set; }
}
