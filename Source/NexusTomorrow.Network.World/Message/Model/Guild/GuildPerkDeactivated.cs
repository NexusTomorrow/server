using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Deactivates one of a guild's perks.
/// </summary>
/// <remarks>
/// The client removes the perk from the active ones and raises GuildPerkDeactivated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04BA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildPerkDeactivated
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The perk, by id.
    /// </summary>
    [UInt(14)] public ushort PerkId { get; set; }
}
