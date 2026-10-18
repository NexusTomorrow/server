using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Answers a <see cref="MovementControl"/>: the client sends it as soon as it takes one, before it acts on it.
/// </summary>
/// <remarks>
/// The client registers a reader for it as well, but does nothing with one it receives.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0635, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MovementControlReceipt
{
    /// <summary>
    /// The <see cref="MovementControl.Ticket"/> of the <see cref="MovementControl"/> it answers.
    /// </summary>
    [UInt(32)] public uint Ticket { get; private set; }
}
