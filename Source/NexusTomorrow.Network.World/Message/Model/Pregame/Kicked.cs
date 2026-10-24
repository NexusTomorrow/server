using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The server ends the player's session.
/// </summary>
/// <remarks>
/// Taken on character select, realm select and in the world: the client drops its connections and shows a message.
/// </remarks>
[WireMessage(GameMessageOpcode.Server036A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class Kicked
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
