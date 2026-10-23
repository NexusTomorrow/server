using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Unlocks one of a guild's perks.
/// </summary>
/// <remarks>
/// The client raises GuildPerkUnlocked.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04BD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildPerkUnlocked
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
