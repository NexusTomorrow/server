using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// Clears the player's ability book cache and raises AbilityBookChange, rebuilding the cache from the player when the flag is set.
/// </summary>
[WireMessage(GameMessageOpcode.Server00B2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ActionSetClearCache
{
    /// <summary>
    /// When set, the client rebuilds the cleared ability cache from the player before raising AbilityBookChange.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Rebuild { get; set; }
}
