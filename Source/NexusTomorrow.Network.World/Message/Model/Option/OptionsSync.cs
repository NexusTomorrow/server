using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// Restates the player's options as the client enters a world: it sends one each time <see cref="WorldEntry.WorldSettings"/> arrives,
/// with its casting options and both combat log choices.
/// </summary>
[WireMessage(GameMessageOpcode.Client00D5, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class OptionsSync
{
    /// <summary>
    /// The casting options, as <see cref="OptionSet.Value"/> carries them with <see cref="OptionSetType.InputFlags"/>: the low four bits of
    /// the client's mask.
    /// </summary>
    [VerifiedMember, UInt(4)] public InputFlags InputFlags { get; set; }

    /// <summary>
    /// Whether the combat log hides other players' entries, as <see cref="CombatLogOtherPlayersSet.DisableOtherPlayers"/> carries it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool CombatLogDisableOtherPlayers { get; set; }

    /// <summary>
    /// The hidden combat log entry types, as <see cref="CombatLogTypesSet.DisabledTypes"/> carries them: bits 0 to 13.
    /// </summary>
    [VerifiedMember, UInt(14)] public CombatLogType CombatLogDisabledTypes { get; set; }
}
