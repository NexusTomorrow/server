using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the player's health.
/// </summary>
/// <remarks>
/// The client raises UI_HealthChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server092F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerHealthChanged
{
    /// <summary>
    /// The player's unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The player's health.
    /// </summary>
    [UInt(32)] public uint Health { get; set; }

    [UInt(18)] public uint Unknown2 { get; set; }
}
