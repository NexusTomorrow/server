using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// Clears the player's ability book cache and raises AbilityBookChange; when the flag is set, the client then raises CombatLogLAS
/// for the player as well.
/// </summary>
[WireMessage(GameMessageOpcode.Server00B2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ActionSetClearCache
{
    /// <summary>
    /// When set, the client raises CombatLogLAS for the player after AbilityBookChange, and the shipped combat log add-on posts a
    /// line for it to the combat channel. When clear, the client raises only AbilityBookChange.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool PostToCombatLog { get; set; }
}
