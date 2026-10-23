using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sheathes or draws the player's weapons.
/// </summary>
[WireMessage(GameMessageOpcode.Client0177, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class WeaponsSheathe
{
    /// <summary>
    /// True puts the weapons away, false draws them.
    /// </summary>
    [UInt(1)] public bool Sheathe { get; private set; }
}
