using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Fills a guild bank tab with its items.
/// </summary>
/// <remarks>
/// The client marks the tab loaded and raises GuildBankTab with the tab's 1-based index.
/// </remarks>
[WireMessage(GameMessageOpcode.Server047A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildBankTabItems
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The tab, as an inventory location: 100 to 109 a guild's tabs, 200 to 209 a war party's.
    /// </summary>
    [UInt(9)] public ushort Location { get; set; }

    /// <summary>
    /// The tab's items, each with its slot.
    /// </summary>
    [Array(CountBits = 32)] public List<Server0111Part1> Items { get; set; } = [];
}
