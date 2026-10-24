using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Tells the client a <see cref="CharacterSelect"/> failed; the client returns to idle and passes the reason to its interface.
/// </summary>
/// <remarks>
/// Taken only while a selection is pending.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0162, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterSelectFailed
{
    /// <summary>
    /// The client picks the interface's select error text by this value: 1 shows the rename-first text, 3 the disabled text and 2 the
    /// general text.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Unknown0 { get; set; }
}
