using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Tells the client a <see cref="CharacterSelect"/> failed; the client returns to idle and passes the reason to its interface.
/// </summary>
[WireMessage(GameMessageOpcode.Server0162, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterSelectFailed
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
