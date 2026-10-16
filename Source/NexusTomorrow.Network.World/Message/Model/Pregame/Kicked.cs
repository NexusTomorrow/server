using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The server ends the player's session.
/// </summary>
/// <remarks>
/// Taken on character select, realm select and in the world: the client drops its connections and shows the reason's message.
/// </remarks>
[WireMessage(GameMessageOpcode.Server036A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class Kicked
{
    /// <summary>
    /// Why the session ended: 6, 7, 8, 12, 15, 16, 19 and 20 each have a message of their own, any other value a general message.
    /// </summary>
    [UInt(5)] public byte Reason { get; set; }
}
