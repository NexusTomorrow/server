using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets one of the player's values, by kind: a currency, the item proficiencies, combat momentum or the wake-here cooldown.
/// </summary>
[WireMessage(GameMessageOpcode.Server0919, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerValueChanged
{
    /// <summary>
    /// The kind: 0 to 13 currencies 1 to 14, 22 the item proficiencies, 23 to 28 combat momentum, 29 the wake-here
    /// cooldown, and 30 currency 15.
    /// </summary>
    [UInt(5)] public byte Kind { get; set; }

    /// <summary>
    /// The value.
    /// </summary>
    [UInt(64)] public ulong Value { get; set; }

    /// <summary>
    /// For combat momentum, a CombatReward table id.
    /// </summary>
    [UInt(32)] public uint CombatRewardId { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
