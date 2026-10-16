using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The player's place in the login queue, which the client passes to the character-select interface.
/// </summary>
[WireMessage(GameMessageOpcode.Server0717, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class LoginQueueStatus
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
