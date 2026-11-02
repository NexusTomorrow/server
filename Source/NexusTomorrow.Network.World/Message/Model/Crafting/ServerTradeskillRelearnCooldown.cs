using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

/// <summary>
/// The tradeskill relearn cooldown; the client sets its relearn deadline from it.
/// </summary>
[WireMessage(GameMessageOpcode.Server0861, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerTradeskillRelearnCooldown
{
    /// <summary>
    /// The relearn cooldown in seconds; the client turns it into a relearn deadline.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Cooldown { get; set; }
}
