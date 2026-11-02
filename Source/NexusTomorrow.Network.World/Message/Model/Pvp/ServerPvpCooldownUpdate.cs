using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

/// <summary>
/// The PvP cooldown the player is now under; the client recomputes the player's PvP flags from it.
/// </summary>
[WireMessage(GameMessageOpcode.Server013E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerPvpCooldownUpdate
{
    /// <summary>
    /// The PvP cooldown duration the player is under; the client updates the player's PvP flags from it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Cooldown { get; set; }
}
