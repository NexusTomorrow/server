using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the player has dashed.
/// </summary>
[WireMessage(GameMessageOpcode.Client00DE, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class Dash
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
