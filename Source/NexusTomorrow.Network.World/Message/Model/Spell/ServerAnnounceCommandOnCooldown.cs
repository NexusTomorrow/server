using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// The cooldown an announce command is now on; the client shows it as a chat line.
/// </summary>
[WireMessage(GameMessageOpcode.Server0832, Direction.ServerToClient)]
[Verified(Rung.Fields)]
[LayoutConfirmed]
public sealed partial class ServerAnnounceCommandOnCooldown
{
    /// <summary>
    /// The cooldown in milliseconds; the client divides it by 1000 to show seconds.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Cooldown { get; set; }
}