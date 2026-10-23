using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Interacts with a unit, opening the window its interaction calls for, such as the bank, the taxi map or the barber.
/// </summary>
/// <remarks>
/// The client keeps the unit and the interaction, and closes the previous interaction's window.
/// </remarks>
[WireMessage(GameMessageOpcode.Client07EA, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class UnitInteract
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }

    /// <summary>
    /// The kind of interaction, which picks the window the client opens.
    /// </summary>
    [UInt(7)] public byte Interaction { get; private set; }
}
