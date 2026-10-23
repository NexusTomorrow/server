using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the guilds a unit belongs to.
/// </summary>
/// <remarks>
/// The client raises UnitMemberOfGuildChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04D4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitGuildsChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's guilds, by their ids within the realm.
    /// </summary>
    [Array(CountBits = 8), UInt(64)] public List<ulong> GuildIds { get; set; } = [];
}
