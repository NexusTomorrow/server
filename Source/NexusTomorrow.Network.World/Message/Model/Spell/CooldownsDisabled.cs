using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Turns the player's spell cooldowns off or on.
/// </summary>
/// <remarks>
/// The client clears the player's cooldowns.
/// </remarks>
[WireMessage(GameMessageOpcode.Server080E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CooldownsDisabled
{
    /// <summary>
    /// True when cooldowns are off.
    /// </summary>
    [UInt(1)] public bool Disabled { get; set; }
}
