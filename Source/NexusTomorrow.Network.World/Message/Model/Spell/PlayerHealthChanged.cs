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
[Verified(Rung.Fields)]
public sealed partial class PlayerHealthChanged
{
    /// <summary>
    /// The player's unit, by unit id.
    /// </summary>
    [VerifiedMember]
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The player's health.
    /// </summary>
    [VerifiedMember]
    [UInt(32)] public uint Health { get; set; }

    /// <summary>
    /// Status bits. Bit 0x200 (bit 9) triggers the breath flash; bit 0x40 (bit 6) and bit 9 gate the DamageOrHealingDone event.
    /// </summary>
    [VerifiedMember]
    [UInt(18)] public uint Flags { get; set; }
}
