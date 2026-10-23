using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the player's spell cooldowns.
/// </summary>
/// <remarks>
/// The client stops at the first cooldown with no duration.
/// </remarks>
[WireMessage(GameMessageOpcode.Server091B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CooldownList
{
    /// <summary>
    /// The cooldowns.
    /// </summary>
    [Array(CountBits = 32)] public List<SpellCooldown> Cooldowns { get; set; } = [];
}
