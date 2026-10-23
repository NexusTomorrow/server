using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Shows a soldier whack-a-mole event's new burrows.
/// </summary>
/// <remarks>
/// The client raises PlayerPathSoldierNewWhackAMoleBurrows with the delay in seconds and the units its ids find.
/// </remarks>
[WireMessage(GameMessageOpcode.Server067E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierWhackAMoleBurrows
{
    /// <summary>
    /// The time until the burrows' units pop up, in milliseconds.
    /// </summary>
    [UInt(32)] public uint DelayMs { get; set; }

    /// <summary>
    /// The burrows' units, by unit id.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public uint[] UnitIds { get; set; } = [];
}
