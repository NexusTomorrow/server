using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Starts or updates one of the player's spell cooldowns.
/// </summary>
[WireMessage(GameMessageOpcode.Server0804, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellCooldownStart
{
    /// <summary>
    /// The cooldown.
    /// </summary>
    public SpellCooldown Cooldown { get; set; } = new();
}
